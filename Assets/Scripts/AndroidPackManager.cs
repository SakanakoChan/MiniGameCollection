using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AndroidPackManager : MonoBehaviour
{
    [ContextMenu("Get jar pack")]
    public void GetJarPack()
    {
        string jarLocation = "E:\\GameProject\\AndroidProject\\MiniGameCollection_AndroidFunction\\MiniGameCollection\\build\\intermediates\\aar_main_jar\\debug\\syncDebugLibJars\\classes.jar";
        string targetLocation = "E:\\GameProject\\Unity\\MiniGameCollection\\Assets\\Plugins\\Android\\libs\\SDKLibrary.jar";
        File.Copy(jarLocation, targetLocation, true);
        Debug.Log("Jar pack copied!");
    }
}
