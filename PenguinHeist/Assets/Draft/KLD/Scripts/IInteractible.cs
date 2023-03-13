public interface IInteractible
{
    public void Interact(PlayerInteraction _playerInteraction);

    public float GetInteractionDuration();

    public bool IsInteractable();
}
