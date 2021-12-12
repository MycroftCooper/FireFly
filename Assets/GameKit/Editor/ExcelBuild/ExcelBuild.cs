using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
#if UNITY_EDITOR
namespace EditorTool {
    public class ExcelBuild : Editor {
        // [MenuItem("CustomEditor/LoadItemAsset")]
        public static void LoadItemAsset() {
            // List<Item> dataPool = new List<Item>(ExcelTool.CreateItemSOWithExcel(ExcelConfig.excelPath + "Item.xlsx"));
            // ItemPool.Instance.pool.Clear();
            // ItemPool.Instance.pool = dataPool;
            // 确保文件夹存在
            // if(!Directory.Exists(ExcelConfig.scriptableObjectPath)) {
            //     Directory.CreateDirectory(ExcelConfig.scriptableObjectPath);
            // }

            // asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            // string assetPath = string.Format("{0}{1}.asset", ExcelConfig.scriptableObjectPath, "ItemModel");
            // 生成一个Asset文件
            // AssetDatabase.CreateAsset(itemModel, assetPath);
            // AssetDatabase.SaveAssets();
            // AssetDatabase.Refresh();
        }
    }
}
#endif
