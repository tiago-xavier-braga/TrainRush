using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// TrafficLightController gerencia o estado de um semáforo e expõe eventos Unity para controlar o trem.
/// 
/// COMO USAR NO INSPECTOR:
/// 1. Adicione este componente a um GameObject na cena.
/// 2. Configure as durações (greenDuration, yellowDuration, redDuration) no Inspector.
/// 3. Configure se o ciclo deve iniciar automaticamente (startCycleOnStart).
/// 4. Conecte os UnityEvents aos métodos de outros scripts:
///    - OnTrainCanMove: conecte ao método StartMovement do TrainController (ex: Departing no TrainMovementController)
///    - OnTrainStop: conecte ao método StopMovement do TrainController (ex: WaitingForSignal no TrainMovementController)
///    - OnGreen, OnYellow, OnRed: use para mudar cor visual do semáforo ou disparar efeitos
///    - OnStateChanged: recebe o nome do estado atual como string
/// 5. Use o Context Menu (botão direito no componente) para testar: "Force Green", "Force Yellow", "Force Red", "Start Cycle", "Stop Cycle"
/// </summary>
public class TrafficLightController : MonoBehaviour
{
    /// <summary>
    /// Estados possíveis do semáforo
    /// </summary>
    public enum TrafficLightState
    {
        Red,
        Yellow,
        Green
    }

    [Header("Durations Configuration")]
    [Tooltip("Duração do estado verde em segundos")]
    [SerializeField]
    [Min(0.1f)]
    private float greenDuration = 5f;

    [Tooltip("Duração do estado amarelo em segundos")]
    [SerializeField]
    [Min(0.1f)]
    private float yellowDuration = 2f;

    [Tooltip("Duração do estado vermelho em segundos")]
    [SerializeField]
    [Min(0.1f)]
    private float redDuration = 5f;

    [Header("Auto-Start Configuration")]
    [Tooltip("Se verdadeiro, o ciclo automático inicia no Start()")]
    [SerializeField]
    private bool startCycleOnStart = true;

    [Header("Current State (Read-Only)")]
    [Tooltip("Estado atual do semáforo")]
    [SerializeField]
    private TrafficLightState currentState = TrafficLightState.Red;

    [Header("Unity Events - State Changes")]
    [Tooltip("Disparado quando o semáforo fica verde")]
    public UnityEvent OnGreen;

    [Tooltip("Disparado quando o semáforo fica amarelo")]
    public UnityEvent OnYellow;

    [Tooltip("Disparado quando o semáforo fica vermelho")]
    public UnityEvent OnRed;

    [Header("Unity Events - Train Control")]
    [Tooltip("Disparado quando o trem pode se mover (estado verde)")]
    public UnityEvent OnTrainCanMove;

    [Tooltip("Disparado quando o trem deve parar (estado vermelho)")]
    public UnityEvent OnTrainStop;

    [Header("Unity Events - State Info")]
    [Tooltip("Disparado em qualquer mudança de estado, enviando o nome do estado como string")]
    public UnityEvent<string> OnStateChanged;

    /// <summary>
    /// Propriedade read-only para verificar se o semáforo está verde
    /// </summary>
    public bool IsGreen => currentState == TrafficLightState.Green;

    /// <summary>
    /// Referência à corrotina do ciclo automático
    /// </summary>
    private Coroutine cycleCoroutine;

    private void Awake()
    {
        // Inicializa os UnityEvents para evitar NullReferenceException
        if (OnGreen == null)
            OnGreen = new UnityEvent();
        if (OnYellow == null)
            OnYellow = new UnityEvent();
        if (OnRed == null)
            OnRed = new UnityEvent();
        if (OnTrainCanMove == null)
            OnTrainCanMove = new UnityEvent();
        if (OnTrainStop == null)
            OnTrainStop = new UnityEvent();
        if (OnStateChanged == null)
            OnStateChanged = new UnityEvent<string>();
    }

    private void Start()
    {
        // Inicia o ciclo automático se configurado
        if (startCycleOnStart)
        {
            StartCycle();
        }
    }

    /// <summary>
    /// Força o semáforo para o estado verde
    /// </summary>
    [ContextMenu("Force Green")]
    public void ForceGreen()
    {
        StopCycle();
        SetState(TrafficLightState.Green);
    }

    /// <summary>
    /// Força o semáforo para o estado amarelo
    /// </summary>
    [ContextMenu("Force Yellow")]
    public void ForceYellow()
    {
        StopCycle();
        SetState(TrafficLightState.Yellow);
    }

    /// <summary>
    /// Força o semáforo para o estado vermelho
    /// </summary>
    [ContextMenu("Force Red")]
    public void ForceRed()
    {
        StopCycle();
        SetState(TrafficLightState.Red);
    }

    /// <summary>
    /// Inicia o ciclo automático de estados do semáforo
    /// </summary>
    [ContextMenu("Start Cycle")]
    public void StartCycle()
    {
        // Para o ciclo anterior se houver
        if (cycleCoroutine != null)
        {
            StopCoroutine(cycleCoroutine);
        }

        // Inicia novo ciclo
        cycleCoroutine = StartCoroutine(AutomaticCycleCoroutine());
    }

    /// <summary>
    /// Para o ciclo automático de estados do semáforo
    /// </summary>
    [ContextMenu("Stop Cycle")]
    public void StopCycle()
    {
        if (cycleCoroutine != null)
        {
            StopCoroutine(cycleCoroutine);
            cycleCoroutine = null;
        }
    }

    /// <summary>
    /// Corrotina que gerencia o ciclo automático dos estados
    /// Ciclo: Verde -> Amarelo -> Vermelho -> Verde...
    /// </summary>
    private IEnumerator AutomaticCycleCoroutine()
    {
        while (true)
        {
            // Estado Verde
            SetState(TrafficLightState.Green);
            yield return new WaitForSeconds(greenDuration);

            // Estado Amarelo
            SetState(TrafficLightState.Yellow);
            yield return new WaitForSeconds(yellowDuration);

            // Estado Vermelho
            SetState(TrafficLightState.Red);
            yield return new WaitForSeconds(redDuration);
        }
    }

    /// <summary>
    /// Define o estado atual do semáforo e dispara os eventos apropriados
    /// </summary>
    /// <param name="newState">Novo estado do semáforo</param>
    private void SetState(TrafficLightState newState)
    {
        currentState = newState;

        // Dispara eventos específicos do estado
        switch (newState)
        {
            case TrafficLightState.Green:
                OnGreen?.Invoke();
                OnTrainCanMove?.Invoke();
                break;

            case TrafficLightState.Yellow:
                OnYellow?.Invoke();
                OnTrainStop?.Invoke();
                break;

            case TrafficLightState.Red:
                OnRed?.Invoke();
                OnTrainStop?.Invoke();
                break;
        }

        // Sempre dispara o evento de mudança de estado com o nome do estado
        OnStateChanged?.Invoke(newState.ToString());
    }
}
