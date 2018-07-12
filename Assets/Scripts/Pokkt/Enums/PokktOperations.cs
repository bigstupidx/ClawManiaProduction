using System;

namespace Pokkt
{
	internal enum PokktOperations
	{
		// Common Methods
		SetParams,
		SetDebug,
		ShowLog,
		ShowToast,
		SetThirdPartyUserId,
		
		// Session Methods
		StartSession,
		EndSession,
		
		// Offerwall Methods
		SetCloseOnSuccessFlag,
		GetCoins,
		GetPendingCoins,
		CheckOfferWallCampaign,
		
		// Video Methods
		GetVideo,
		GetVideoNonIncent,
		CacheVideoCampaign,
		SetSkipEnabled,
		SetDefaultSkipTime,
		SetCustomSkipMessage,
		SetBackButtonDisabled,
		
		// Optional Methods
		SetName,
		SetAge,
		SetSex,
		SetMobileNo,
		SetEmailAddress,
		SetLocation,
		SetBirthday,
		SetMaritalStatus,
		SetFacebookId,
		SetTwitterHandle,
		SetEducation,
		SetNationality,
		SetEmployment,
		SetMaturityRating
	}
}

