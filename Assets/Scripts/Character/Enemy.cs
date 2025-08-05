namespace Character
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class Enemy : FightingObject
    {
        public int dropCount = 3;
        public float attackDistance;
        public float agroDistance;
        public float playerDistance;
        public bool isFlankingLeft, isFlankingRight;
        public GameObject enemyObject;
        public GameObject coinPrefab;
        public float movementRandom;
        public ObjectiveManager objectiveManager;

        new void Start()
        {
            base.Start();
            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody2D>();
            colliderBox = GetComponent<PolygonCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            movementRandom = Random.Range(-1f, 1f);
            speed += Random.Range(-0.01f, 0.01f);
            objectiveManager = FindObjectOfType<ObjectiveManager>();
        }

        void Update()
        {
            playerDistance = GetDistance(Globals.player);
        }

        // TODO Re-implement flaking behaviour
        void FixedUpdate()
        {
            Walk();
            // FlankRight();
            // FlankLeft();
            Slash();
            UpdatePolygonCollider2D();
            Die();
        }

        void Slash()
        {
            if (IsInAttackDistance() && isFlankingLeft == false && isFlankingRight == false)
            {
                animator.SetTrigger("slash");
            }
        }

        void Walk()
        {
            if (ShouldWalk())
            {
                if (IsDirectionRight())
                {
                    gameObject.transform.position += new Vector3(speed, 0, 0);
                    spriteRenderer.flipX = false;
                    animator.SetBool("walking", true);
                }
                else if (IsDirectionLeft())
                {
                    gameObject.transform.position -= new Vector3(speed, 0, 0);
                    spriteRenderer.flipX = true;
                    animator.SetBool("walking", true);
                }
            }
            else
            {
                animator.SetBool("walking", false);
            }
        }

        private bool ShouldWalk()
        {
            return IsInAgroDistance() && !IsInAttackDistance() && !isFlankingLeft &&
                   !isFlankingRight;
        }

        private bool IsInAttackDistance()
        {
            return Mathf.Abs(playerDistance) < attackDistance - movementRandom;
        }

        public void OnTriggerStay2D(Collider2D collision)
        {
            Debug.Log("OnTriggerStay2D");
            if (collision.CompareTag("Player"))
                enemyObject = collision.gameObject;
        }

        public override void HitEnemy()
        {
            if (enemyObject == null || !enemyObject.CompareTag("Player")) return;
            var isCrit = Random.value < crit;
            var dmgAmount = damage * (isCrit ? 2 : 1) - Globals.player.defense;
            if (dmgAmount < 1)
            {
                dmgAmount = 1;
            }

            enemyObject.GetComponent<FightingObject>().TakeDamage(dmgAmount, isCrit);
        }

        private bool IsLeftClear()
        {
            var hits = Physics2D.RaycastAll(transform.position, Vector2.left, 2);
            var enemies = hits
                .Where(hit => hit.transform.CompareTag("Enemy"))
                .Select(hit => hit.collider.gameObject)
                .Where(go =>
                {
                    var enemy = go.GetComponent<Enemy>();
                    return !enemy.isFlankingLeft && !enemy.isFlankingRight;
                })
                .ToList();
            return enemies.Count < 2;
        }

        private bool IsRightClear()
        {
            var hits = Physics2D.RaycastAll(transform.position, Vector2.right, 2);
            var enemies = hits
                .Where(hit => hit.transform.CompareTag("Enemy"))
                .Select(hit => hit.collider.gameObject)
                .Where(hit =>
                {
                    var enemy = hit.gameObject.GetComponent<Enemy>();
                    return !enemy.isFlankingLeft && !enemy.isFlankingRight;
                })
                .ToList();
            return enemies.Count < 2;
        }

        private bool IsDirectionRight()
        {
            return playerDistance > 0;
        }

        private bool IsDirectionLeft()
        {
            return !IsDirectionRight();
        }

        IEnumerator StopFlanking()
        {
            float seconds = Mathf.Abs(GetDistance(Globals.player)) * 2 / (0.08f * 50);
            yield return new WaitForSeconds(seconds);
            isFlankingLeft = false;
            isFlankingRight = false;
            spriteRenderer.flipX = GetDistance(Globals.player) <= 0;
        }

        private void DropLoot()
        {
            for (int i = 0; i < dropCount; i++)
            {
                Instantiate(coinPrefab, gameObject.transform.position, Quaternion.identity);
            }
        }

        protected override void Die()
        {
            if (health > 0) return;
            DropLoot();
            objectiveManager.enemies.Remove(this);
            Destroy(gameObject);
        }

        private bool IsInAgroDistance()
        {
            return GetAbsoluteDistance(Globals.player) < agroDistance;
        }
    }
}