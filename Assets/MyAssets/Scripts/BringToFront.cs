using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringToFront : MonoBehaviour {
	void onEnable()
    {
        transform.SetAsLastSibling();
    }
}
