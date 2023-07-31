﻿using System.Collections.Generic;
using ApiBlueprint.DataAccess.Connection;
using ApiBlueprint.DataAccess.Contracts;

namespace ApiBlueprint.DataAccess.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> 
	where TEntity : class
{
	protected readonly DatabaseContext Context;

    protected RepositoryBase(DatabaseContext context)
	{
		Context = context;
	}

	public void Add(TEntity entity)
	{
		Context.Set<TEntity>().Add(entity);
	}

	public void AddRange(IEnumerable<TEntity> entities)
	{
		Context.Set<TEntity>().AddRange(entities);
	}

	public void Remove(TEntity entity)
	{
		Context.Set<TEntity>().Remove(entity);
	}

	public void RemoveRange(IEnumerable<TEntity> entities)
	{
		Context.Set<TEntity>().RemoveRange(entities);
	}
}
