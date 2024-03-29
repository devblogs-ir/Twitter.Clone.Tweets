﻿using Mapster;
using Twitter.Clone.Tweets.Models.Domain;

namespace Twitter.Clone.Tweets.Models.Contracts.Profiles;

public class TweetRegisterConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateTweetRequest, Tweet>()
            .Map(x => x.Content, z => z.text)
            .Map(x => x.UserId, z => Guid.NewGuid());
    }
}
