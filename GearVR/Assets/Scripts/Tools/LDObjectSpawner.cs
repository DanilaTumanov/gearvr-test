using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Компонент для удобной расстановки объектов
/// </summary>
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class LDObjectSpawner : BaseObjectScene {
    
    [Header("Основные настройки")]
	public GameObject prefab;
    public Transform parent;

    [Header("Вращение")]
    public bool randomXRotation = false;
    public bool randomYRotation = false;
    public bool randomZRotation = false;
    public Vector3 rotation = Vector3.zero;

}
	