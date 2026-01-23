using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class PelotaInteractiva : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    private Rigidbody rb;
    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        // Guardamos la posición y rotación inicial
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Transferir la velocidad y angular velocity del interactor (mano VR)
        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor)
        {
            rb.linearVelocity = interactor.GetAttachTransform(this).GetComponent<Rigidbody>()?.linearVelocity ?? Vector3.zero;
            rb.angularVelocity = interactor.GetAttachTransform(this).GetComponent<Rigidbody>()?.angularVelocity ?? Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody otherRb = collision.collider.GetComponent<Rigidbody>();
        if (otherRb != null)
        {
            otherRb.isKinematic = false;
            otherRb.useGravity = true;
        }

        // Iniciar respawn después de colisión
        StartCoroutine(Reaparecer());
    }

    private IEnumerator Reaparecer()
    {
        // Desactivar la pelota
        gameObject.SetActive(false);

        // Esperar 5 segundos
        yield return new WaitForSeconds(5f);

        // Resetear posición y rotación
        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Reactivar la pelota
        gameObject.SetActive(true);
    }
}