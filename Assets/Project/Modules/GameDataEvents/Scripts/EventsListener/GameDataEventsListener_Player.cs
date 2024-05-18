namespace Popeye.Modules.GameDataEvents
{
    public partial class GameDataEventsListener
    {
        private void OnPlayerRest(OnPlayerRestEvent eventInfo)
        {
            PlayerRestEventData eventData =
                new PlayerRestEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: PlayerRestEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                position: eventData.Position.ToStringParsed()
            );

            _eventsConsumer.AddEventContent(eventContent);
        }

        private void OnPlayerTakeDamage(OnPlayerTakeDamageEvent eventInfo)
        {
            PlayerTakeDamageEventData eventData =
                new PlayerTakeDamageEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: PlayerTakeDamageEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                position: eventData.Position.ToStringParsed(),
                damageCause: eventData.DamageHitName,
                playerHealthCurrent: eventData.CurrentHealth.ToString(),
                wasKilled: eventData.WasKilled.ToString()
                );

            _eventsConsumer.AddEventContent(eventContent);
        }
        
        private void OnPlayerKilledByDamage(OnPlayerKilledByDamageEvent eventInfo)
        {
            PlayerKilledByDamageEventData eventData =
                new PlayerKilledByDamageEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: PlayerKilledByDamageEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                position: eventData.Position.ToStringParsed(),
                damageCause: eventData.DamageHitName
            );

            _eventsConsumer.AddEventContent(eventContent);
        }

        private void OnPlayerHeal(OnPlayerHealEvent eventInfo)
        {
            PlayerHealEventData eventData =
                new PlayerHealEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: PlayerHealEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                position: eventData.Position.ToStringParsed(),
                playerHealthCurrent: eventData.CurrentHealth.ToString(),
                playerHealthBeforeEvent: eventData.HealthBeforeHealing.ToString());

            _eventsConsumer.AddEventContent(eventContent);
        }

        private void OnPlayerUpdate(OnPlayerUpdateEvent eventInfo)
        {
            PlayerUpdateEventData eventData =
                new PlayerUpdateEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: PlayerUpdateEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                position: eventData.Position.ToStringParsed());

            _eventsConsumer.AddEventContent(eventContent);
        }

        private void OnPlayerAction(OnPlayerActionEvent eventInfo)
        {
            PlayerActionEventData eventData =
                new PlayerActionEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: PlayerActionEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                playerActionType: eventData.ActionName);

            _eventsConsumer.AddEventContent(eventContent);
        }


        private void OnEnterPuzzle(OnPuzzleEnterEvent eventInfo)
        {
            PuzzleEnterEventData eventData =
                new PuzzleEnterEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: PuzzleEnterEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                id: eventData.Id);

            _eventsConsumer.AddEventContent(eventContent);
        }
        private void OnExitPuzzle(OnPuzzleExitEvent eventInfo)
        {
            PuzzleExitEventData eventData =
                new PuzzleExitEventData(GetNewGenericEventData(), eventInfo);

            string eventContent = MakeContentFromEventData(
                eventName: PuzzleExitEventData.NAME,
                timeStamp: eventData.GenericEventData.TimeStamp,
                sceneName: eventData.GenericEventData.SceneName,
                id: eventData.Id);

            _eventsConsumer.AddEventContent(eventContent);
        }
        
    }
}