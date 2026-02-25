using System;
using System.Collections.Generic;
using Mechanics.Characters;
using Mechanics.DayNight;

namespace Mechanics.Product
{
    public interface IContractMarketService : IService
    {
        event Action OnOffersRefreshed;
        event Action OnOfferTaken;

        IReadOnlyList<IContractData> GetAvailableOffers();
        bool CanTakeOffer(IContractData offer);
        void TakeOffer(IContractData offer, List<ICharacterData> assignees, string ownerKey);
        bool IsTierUnlocked(ContractTier tier);
    }
}
