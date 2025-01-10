using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yapa.Common.Types;
using Yapa.Features.NoteTaking.Types;

namespace Yapa.Features.NoteTaking;

public sealed class CollectionService
{
    private readonly ICollectionRepository _collectionRepository;

    public CollectionService(ICollectionRepository collectionRepository)
    {
        _collectionRepository = collectionRepository;
    }

    public async Task<Result<List<CollectionDto>>> GetAll()
    {
        var collectionRecords = await _collectionRepository.GetAll();
        
        return Result<List<CollectionDto>>.Success(collectionRecords);
    }

    public async Task<Result<CollectionDto>>AddCollection(string collectionName)
    {
        if(string.IsNullOrWhiteSpace(collectionName))
            return Result<CollectionDto>.Failure("Collection name cannot be empty");
            
        var collectionRecord = new CollectionDto()
        {
            Id = Guid.NewGuid(),
            Name = collectionName,
            IsArchived = false,
        };
        await _collectionRepository.Add(collectionRecord);
        return Result<CollectionDto>.Success(collectionRecord);
    }
}