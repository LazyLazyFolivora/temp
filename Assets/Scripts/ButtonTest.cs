using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    public Button btn;
    int i = 1;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(delegate
        {
            BGMManager.GetInstance.ChangeBGM("test" + (i % 4 + 1));
            i++;
            // ����Auto����ͼ
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
