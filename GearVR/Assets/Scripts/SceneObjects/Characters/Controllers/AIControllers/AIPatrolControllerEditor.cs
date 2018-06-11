using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
[CustomEditor(typeof(AIPatrolController))]
public class AIPatrolControllerEditor : UnityEditor.Editor
{

    const string ADD_ROUTE_TEXT = "Добавить маршрут";
    const string CHANGE_ROUTE_TEXT = "Изменить маршрут";
    const string SAVE_ROUTE_TEXT = "Сохранить маршрут";
    const string CANCEL_ADD_ROUTE_TEXT = "Отменить";
        
        
    // Устанавливается ли маршрут (после нажатия кнопки "добавить маршрут")
    private bool _addingRoute = false;
    private Vector3 _mousePosition;
    private AIPatrolController _target;
    private GameObject _route;

        

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        _target = (AIPatrolController) target;


        Selection.selectionChanged = ClearNewRoute;

        if (GUILayout.Button(_addingRoute ? SAVE_ROUTE_TEXT : (_target.patrolRoute != null ? CHANGE_ROUTE_TEXT : ADD_ROUTE_TEXT), EditorStyles.miniButton))
        {
            _addingRoute = !_addingRoute;

            if (_addingRoute)
            {
                SetRootEnable();
            }
            else
            {
                SaveRoot();
            }
        }

        if(_addingRoute && GUILayout.Button(CANCEL_ADD_ROUTE_TEXT, EditorStyles.miniButton))
        {
            ClearNewRoute();
            _addingRoute = false;
        }

        if (_addingRoute)
        {
            EditorGUILayout.HelpBox("Добавляйте маршрут на сцене, отмечая точки нажатием Shift + правая кнопка мыши. После завершения нажмите \"Сохранить маршрут\"", MessageType.Info);
        }
    }


    private void OnSceneGUI()
    {

        if (_addingRoute)
        {
            ProcessSetRoute();
        }
            
    }







    private void SetRootEnable()
    {
        Transform parent = GameObject.FindGameObjectWithTag("RouteContainer").transform;
        _route = new GameObject("PatrolRoute_" + _target.name);
        _route.transform.parent = parent;
        _route.isStatic = true;
    }


    private void SaveRoot()
    {
        if(_route.transform.childCount > 0)
        {
            ClearExistingRoute();
            _target.patrolRoute = _route.transform;
            _route = null;
            EditorUtility.SetDirty(_target);
        }
        else
        {
            ClearNewRoute();
        }
    }


    private void ClearNewRoute()
    {
        if (_route == null)
            return;
            
        DestroyImmediate(_route);
        _route = null;
            
    }


    private void ClearExistingRoute()
    {
        if (_target.patrolRoute != null)
        {
            DestroyImmediate(_target.patrolRoute.gameObject);
            _target.patrolRoute = null;
        }
    }


    private void ProcessSetRoute()
    {
        if (Event.current.shift && Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            RaycastHit hit;
            LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");

            _mousePosition.x = Event.current.mousePosition.x;
            _mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - Event.current.mousePosition.y;
            Ray ray = Camera.current.ScreenPointToRay(_mousePosition);

            if (Physics.Raycast(ray, out hit, 10000, layerMask))
            {
                GameObject routePoint = new GameObject("RoutePoint");
                routePoint.transform.parent = _route.transform;
                routePoint.transform.position = hit.point;
                routePoint.isStatic = true;
            }
        }
    }

}
#endif