using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yapa.Common.Types;
using Yapa.Modules.NoteTaking.Types;

namespace Yapa.Modules.NoteTaking;

public sealed class CollectionService
{
    private readonly ICollectionRepository _collectionRepository;
    private readonly TimeProvider _timeProvider;

    public CollectionService(ICollectionRepository collectionRepository, TimeProvider timeProvider)
    {
        _collectionRepository = collectionRepository;
        _timeProvider = timeProvider;
    }

    public async Task<Result<List<CollectionRecord>>> GetAll()
    {
        var collectionRecords = await _collectionRepository.GetAll();

        if (collectionRecords.Count == 0)
            return Result<List<CollectionRecord>>.Failure("No collection records found");
        
        return Result<List<CollectionRecord>>.Success(collectionRecords);
    }

    public async Task<Result<CollectionRecord>>AddCollection(string collectionName)
    {
        var collectionRecord = new CollectionRecord
        {
            Id = Guid.NewGuid(),
            Name = collectionName,
            IsArchived = false
        };
        await _collectionRepository.Add(collectionRecord);
        return Result<CollectionRecord>.Success(collectionRecord);
    }
}