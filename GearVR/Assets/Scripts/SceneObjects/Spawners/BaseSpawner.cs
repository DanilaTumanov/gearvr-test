using System.Collections;
using UnityEngine;


public class BaseSpawner : BaseObjectScene
{
    #region Fields
    /// <summary>
    /// Спаунящийся объект.
    /// </summary>
    [SerializeField]
    [Tooltip("Объект типа BaseObjectScene, который спаунится данным спаунером.")]
    private BaseObjectScene _spawnerObj;

    /// <summary>
    /// Минимальное и максимальное время задержки первого спауна.
    /// </summary>
    [SerializeField]
    [Tooltip("Минимальное и максимальное время задержки первого спауна.")]
    private Vector2 _firstSpawnDelay;

    /// <summary>
    /// Минимальное и максимальное время между спаунами.
    /// </summary>
    [SerializeField]
    [Tooltip("Минимальное и максимальное время между спаунами.")]
    private Vector2 _timeBetweenSpawns;

    /// <summary>
    /// Признак пассивности спаунера.
    /// Если <code>true</code> то объекты спаунятся только по команде.
    /// Если <code>false</code> то объекты спаунятся в автоматическом режиме.
    /// </summary>
    [SerializeField]
    [Tooltip("Если false, то спаунер спаунит объекты в автоматическом режиме. Если true, то объекты спаунятся только по команде.")]
    private bool _isPassive;

    /// <summary>
    /// Признак проверки видимости камерой. 
    /// Если <code>true</code> то объекты спаунятся только когда сапаунер находится вне зоны видимости камеры.
    /// Если <code>false</code> то объекты спаунятся вне зависимости от того находится ли спаунер в зоне видимости камеры.
    /// </summary>
    [SerializeField]
    [Tooltip("Если true, то объекты спаунятся только когда сапаунер находится вне зоны видимости камеры.")]
    private bool _cameraCheck;

    /// <summary>
    /// Признак является ли спаунер видимым для камеры в данный момент.
    /// </summary>
    private bool _isVisible;
    #endregion

   



    #region MB methods
    private void Awake()
    {
        Random.InitState(System.DateTime.Today.Millisecond);
    }

    private void Start()
    {
        if (!_isPassive)
            StartCoroutine(SpawnCoroutine());
    }

    #endregion

    #region Public methods
    /// <summary>
    /// Начать спаунить объекты в автоматическом режиме.
    /// </summary>
    public void StartSpawning()
    {
        _isPassive = false;
        StartCoroutine(SpawnCoroutine());
    }

    /// <summary>
    /// Остановить спаунер.
    /// </summary>
    public void StopSpawning()
    {
        _isPassive = true;
        StopCoroutine(SpawnCoroutine());
    }



    /// <summary>
    /// Выполнить единичный спаун объекта.
    /// </summary>
    public void Spawn()
    {
        if (!(_cameraCheck && _isVisible))
        {
            Instantiate(_spawnerObj, transform.position, transform.rotation);
        }
    }
    #endregion

    #region Private methods
    /// <summary>
    /// Корутина, реализующая спаун объекта, через случайные промежутки времени.
    /// Минимальное и максимальное значение промежутка времени определяется вектором <see cref="TimeBetweenSpawns"/>
    /// </summary>
    private IEnumerator SpawnCoroutine()
    {
        float waitTime = Random.Range(_timeBetweenSpawns.x, _timeBetweenSpawns.y);
        yield return new WaitForSeconds(waitTime);
        while (!_isPassive)
        {
            Spawn();

            //TODO: Повторение кода, убрать
            waitTime = Random.Range(_timeBetweenSpawns.x, _timeBetweenSpawns.y);
            yield return new WaitForSeconds(waitTime);
        }
        yield return null;
    }
    #endregion
}