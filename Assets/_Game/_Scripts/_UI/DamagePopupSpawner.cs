using UnityEngine;

public class DamagePopupSpawner : MonoBehaviour
{
    [SerializeField] private DragonHealth health;

    [Header("Popup")]
    [SerializeField] private DamagePopup popupPrefab;
    [SerializeField] private Transform popupPoint;

    [Header("Pool")]
    [SerializeField] private int poolSize = 20;

    private GenericPool<DamagePopup> popupPool;

    private void Awake()
    {
        popupPool = new GenericPool<DamagePopup>(
            popupPrefab,
            poolSize
        );
    }

    private void OnEnable()
    {
        health.OnDamaged += SpawnPopup;
    }

    private void OnDisable()
    {
        health.OnDamaged -= SpawnPopup;
    }

    private void SpawnPopup(float damage)
    {
        DamagePopup popup =
            popupPool.Get();

        popup.transform.position =
            popupPoint.position;

        FaceCamera(popup.transform);

        popup.Show(
            damage,
            popupPool.Release
        );
    }

    private void FaceCamera(Transform popup)
    {
        if (Camera.main == null)
            return;

        popup.rotation =
            Quaternion.LookRotation(
                popup.position -
                Camera.main.transform.position
            );
    }
}