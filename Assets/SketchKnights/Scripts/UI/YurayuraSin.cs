using UnityEngine;

public class YurayuraSin : MonoBehaviour
{
    [SerializeField] bool isLocal = true;

    [Header("Position")]
    [SerializeField] bool enablePos = true;
    [SerializeField] float posSpeed = 1f;
    [SerializeField] float posAmount = 0.1f;

    [Header("Rotation")]
    [SerializeField] bool enableRot = true;
    [SerializeField] bool isRotZOnly = true;
    [SerializeField] float rotSpeed = 1f;
    [SerializeField] float rotAmount = 10f;

    [Header("Scale")]
    [SerializeField] bool enableScale = true;
    [SerializeField] float scaleSpeed = 1f;
    [SerializeField, Range(0f, 1f)] float scaleAmount = 0.1f;

    Vector3 initialPos;
    Vector3 initialRot;
    Vector3 initialScale;
    float xPosDiff;
    float yPosDiff;
    float zPosDiff;
    float xRotDiff;
    float yRotDiff;
    float zRotDiff;
    float xScaleDiff;
    float yScaleDiff;
    float zScaleDiff;

    void Start()
    {
        Init();
    }

    // 外部と競争しそうな場合、変更の度にこれを外部から呼び出す
    public void Init()
    {
        if(isLocal)
        {
            initialPos = transform.localPosition;
            initialRot = transform.localRotation.eulerAngles;
            initialScale = transform.localScale;
        }
        else
        {
            initialPos = transform.position;
            initialRot = transform.rotation.eulerAngles;

            var parentLossyScale = transform.parent.lossyScale;
            initialScale
                = new Vector3(
                    transform.localScale.x / parentLossyScale.x,
                    transform.localScale.y / parentLossyScale.y,
                    transform.localScale.z / parentLossyScale.z);
        }

        xPosDiff = Random.Range(-1f, 1f) * Mathf.PI * 2f;
        yPosDiff = Random.Range(-1f, 1f) * Mathf.PI * 2f;
        zPosDiff = Random.Range(-1f, 1f) * Mathf.PI * 2f;
        xRotDiff = Random.Range(-1f, 1f) * Mathf.PI * 2f;
        yRotDiff = Random.Range(-1f, 1f) * Mathf.PI * 2f;
        zRotDiff = Random.Range(-1f, 1f) * Mathf.PI * 2f;
        xScaleDiff = Random.Range(-1f, 1f) * Mathf.PI * 2f;
        yScaleDiff = Random.Range(-1f, 1f) * Mathf.PI * 2f;
        zScaleDiff = Random.Range(-1f, 1f) * Mathf.PI * 2f;
    }

    void Update()
    {
        if(enablePos)
        {
            Pos();
        }
        if(enableRot)
        {
            Rot(isRotZOnly);
        }
        if(enableScale)
        {
            Scale();
        }
    }

    void Pos()
    {
        float posTime = Time.time * posSpeed;

        float xPos = Mathf.Sin(posTime + xPosDiff);
        float yPos = Mathf.Sin(posTime + yPosDiff);
        float zPos = Mathf.Sin(posTime + zPosDiff);
        Vector3 pos = initialPos + new Vector3(xPos, yPos, zPos) * posAmount;

        if(isLocal)
        {
            transform.localPosition = pos;
        }
        else
        {
            transform.position = pos;
        }
    }

    void Rot(bool isZOnly = false)
    {
        float rotTime = Time.time * rotSpeed;

        float xRot;
        float yRot;
        if (isZOnly)
        {
            xRot = Mathf.Sin(rotTime + xRotDiff);
            yRot = Mathf.Sin(rotTime + yRotDiff);
        }
        else
        {
            xRot = 0f;
            yRot = 0f;
        }
        float zRot = Mathf.Sin(rotTime + zRotDiff);
        Vector3 rot = initialRot + (new Vector3(xRot, yRot, zRot) * rotAmount);

        if(isLocal)
        {
            transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
    }

    void Scale()
    {
        float scaleTime = Time.time * scaleSpeed;

        float xScale = Mathf.Sin(scaleTime + xScaleDiff) * scaleAmount;
        float yScale = Mathf.Sin(scaleTime + yScaleDiff) * scaleAmount;
        float zScale = Mathf.Sin(scaleTime + zScaleDiff) * scaleAmount;
        Vector3 scale = new Vector3(initialScale.x + xScale, initialScale.y + yScale, initialScale.z + zScale);

        if(isLocal)
        {
            transform.localScale = scale;
        }
        else
        {
            var parentLossyScale = transform.parent.lossyScale;
            transform.localScale
                = new Vector3(
                    scale.x / parentLossyScale.x,
                    scale.y / parentLossyScale.y,
                    scale.z / parentLossyScale.z);
        }
    }
}
