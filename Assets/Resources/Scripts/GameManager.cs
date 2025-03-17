using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static int jumpsCounted = 0;
    public static UnityEvent OnJumpCounted = new UnityEvent();
    public static UnityEvent<int> OnThresholdReached = new UnityEvent<int>();

    [SerializeField] private int[] _thresholds = new int[3];
    public static int[] thresholds = new int[3];

    private void Start()
    {
        OnJumpCounted.AddListener(CheckJumps);

        for(int i = 0; i < _thresholds.Length; i++)
        {
            thresholds[i] = _thresholds[i];
        }
    }

    private void CheckJumps()
    {
        if (jumpsCounted >= thresholds[0] || jumpsCounted >= thresholds[1] || jumpsCounted >= thresholds[2])
        {
            OnThresholdReached.Invoke(jumpsCounted);
        }
    }
}
