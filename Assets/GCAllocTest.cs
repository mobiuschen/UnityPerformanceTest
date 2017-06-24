using UnityEngine;
using System.Collections;

public class GCAllocTest : MonoBehaviour
{
    public int InvokePerFrame = 1000000;

    public bool BoxingGC = false;

    public bool StructGC = false;


    private System.Diagnostics.Stopwatch m_st;

    void Start()
    {
        Debug.LogFormat("Start GC test.");

        m_st = new System.Diagnostics.Stopwatch();
    }


    void Update()
    {
        _BoxingGC();
        _StructGC();
    }


    #region Struct
    private struct ItemDataA
    {
        public int IVvalue;
        public string Name;
        public Vector3 Vec;
    }

	private struct ItemDataB
	{
		public int IVvalue;
		public Vector3 Vec;
	}


    private void _StructGC()
	{
		if (!StructGC)
			goto Exit0;

		bool useRefStruct = Time.frameCount % 2 == 0;
		if (useRefStruct)
		{
            ItemDataA[] ary = new ItemDataA[InvokePerFrame];
			for (int i = 0; i < InvokePerFrame; i++)
			{
                ary[i].Name = "1234567890";
			}
		}
		else
		{
			ItemDataB[] ary = new ItemDataB[InvokePerFrame];
            string[] strAry = new string[InvokePerFrame];
			for (int i = 0; i < InvokePerFrame; i++)
			{
				strAry[i] = "1234567890";
			}
		}


	Exit0:
		return;

    }


    #endregion



    #region Boxing
    private void _BoxingGC()
    {
        if (!BoxingGC)
            goto Exit0;

        bool useBoxing = Time.frameCount % 2 == 0;
        if (useBoxing)
        {
            for (int i = 0; i < InvokePerFrame; i++)
            {
                _TriggerBoxing(i);
            }
        }
        else
        {
            for (int i = 0; i < InvokePerFrame; i++)
            {
                _NoTriggerBoxing(i);
            }
        }


    Exit0:
        return;
    }

    private void _TriggerBoxing(params object[] objs)
    {
        var iValue = (int)objs[0];
        iValue += 1;
    }


    private void _NoTriggerBoxing(int i)
    {
        i = i + 1;
    }

    #endregion
}
