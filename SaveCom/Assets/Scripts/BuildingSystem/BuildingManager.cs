using System;
using System.Collections;
using System.Collections.Generic;
using BuildingSystem.Models;
using Unity.VisualScripting;
using UnityEngine;

namespace BuildingSystem
{
    public class BuildingManager : MonoBehaviour
    {
        public BuildableItem fireWall;
        public BuildableItem sever;
        public BuildableItem[] buildableItems;
        public Vector2 initCoord;
        public Vector2 destCoord;

        private void Start()
        {
            TowerInitial();
            GameSystem.instance.constructionLayer.Build(new Vector3(4, 4, 0), sever);
            GameSystem.instance.constructionLayer.Build(new Vector3(5, 4, 0), sever);
            GameSystem.instance.constructionLayer.Build(new Vector3(4, 5, 0), sever);
            GameSystem.instance.constructionLayer.Build(new Vector3(5, 5, 0), sever);
            for (int i = (int)initCoord.x; i < destCoord.x; i++)
            {
                GameSystem.instance.constructionLayer.Build(new Vector3(i, initCoord.y, 0), fireWall);
            }
            for (int i = (int)initCoord.y; i > destCoord.y; i--)
            {
                GameSystem.instance.constructionLayer.Build(new Vector3(destCoord.x, i, 0), fireWall);
            }
            for (int j = (int)destCoord.x; j > initCoord.x; j--)
            {
                GameSystem.instance.constructionLayer.Build(new Vector3(j, destCoord.y, 0), fireWall);
            }
            for (int j = (int)destCoord.y; j < initCoord.y; j++)
            {
                GameSystem.instance.constructionLayer.Build(new Vector3(initCoord.x, j, 0), fireWall);
            }
            //StartCoroutine(Init());
        }

        IEnumerator Init() // 방화벽 설치
        {
            for (int i = (int)initCoord.x, j = (int)destCoord.x; i < destCoord.x; i++, j--)
            {
                GameSystem.instance.constructionLayer.Build(new Vector3(i, initCoord.y, 0), fireWall);
                yield return null;
                GameSystem.instance.constructionLayer.Build(new Vector3(j, destCoord.y, 0), fireWall);
                yield return new WaitForSeconds(0.02f); 
            }
            for (int i = (int)initCoord.y, j = (int)destCoord.y; i > destCoord.y; i--, j++)
            {
                GameSystem.instance.constructionLayer.Build(new Vector3(destCoord.x, i, 0), fireWall);
                yield return null;
                GameSystem.instance.constructionLayer.Build(new Vector3(initCoord.x, j, 0), fireWall);
                yield return new WaitForSeconds(0.02f);
            }
            
            // for (int i = (int)initCoord.x; i < destCoord.x; i++)
            // {
            //     GameManager.instance.constructionLayer.Build(new Vector3(i, initCoord.y, 0), fireWall);
            //     yield return new WaitForSeconds(0.02f);
            // }
            //
            // for (int i = (int)initCoord.y; i > destCoord.y; i--)
            // {
            //     GameManager.instance.constructionLayer.Build(new Vector3(destCoord.x, i, 0), fireWall);
            //     yield return new WaitForSeconds(0.02f);
            // }
            //
            // for (int j = (int)destCoord.x; j > initCoord.x; j--)
            // {
            //     GameManager.instance.constructionLayer.Build(new Vector3(j, destCoord.y, 0), fireWall);
            //     yield return new WaitForSeconds(0.02f);
            // }
            //
            // for (int j = (int)destCoord.y; j < initCoord.y; j++)
            // {
            //     GameManager.instance.constructionLayer.Build(new Vector3(initCoord.x, j, 0), fireWall);
            //     yield return new WaitForSeconds(0.02f);
            // }
        }

        void TowerInitial() // 스크립터블 오브젝트는 데이터의 변경이 초기화되지 않으므로 초기화 시켜주기 위함
        {
            for (int i = 0; i < buildableItems.Length; i++)
            {
                buildableItems[i].hp = buildableItems[i].init_hp;
                buildableItems[i].damage = buildableItems[i].init_damage;
                buildableItems[i].period = buildableItems[i].init_period;
            }
        }
    }
}
