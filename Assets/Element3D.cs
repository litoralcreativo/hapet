using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Element3D : MonoBehaviour
{
    public Vector2 worldPosition;
    public TextMeshProUGUI text;
    public Image img;

    public Color leftColor;
    public Color rightColor;

    public bool selected = false;

    public Foot foot;
    public SimpleEventType type;

    public Material normalMat;
    public Material selectedMat;

    private MeshRenderer meshRenderer;

    public void Select()
    {
        selected = true;
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material = selectedMat;
    }

    public void Disselect()
    {
        selected = false;

        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material = normalMat;
    }

    public void SetValues()
    {
        if (worldPosition == null)
        {
            text.text = "(null, null)";
        } else
        {
            text.text = worldPosition.ToString();
        }

        if (foot == Foot.left)
        {
            img.color = leftColor;
        } else
        {
            img.color = rightColor;
        }

        if (type == SimpleEventType.contact)
        {
            img.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            img.transform.localScale = new Vector3(1, -1, 1);
        }
    }

}
