using BankData;
using BankData.Entities;
using Microsoft.EntityFrameworkCore;
using OnlineBank.Models;

namespace OnlineBank.Repo
{
	public class TagRepo
	{
		private readonly BankDbContext _context;

		public TagRepo(BankDbContext context)
        {
			_context = context;
		}

		public async Task<List<TagModel>> GetTagsAsync()
		{
			var tags = await _context.Tags.ToListAsync();
			List<TagModel> tagsResult = new();
			tags.ForEach(t=> tagsResult.Add(new TagModel { Id = t.Id , Name = t.Name }));
			return tagsResult;
		}

		public async Task AddTagAsync(TagModel tag)
		{
			Tag newTag = new() { Name = tag.Name };
			await _context.Tags.AddAsync(newTag);
			await _context.SaveChangesAsync(); 
		}

		public async Task DeleteTag(int id)
		{
			var tag = await _context.Tags.FindAsync(id);
			if(tag is not null)
			{
				_context.Tags.Remove(tag);
				await _context.SaveChangesAsync();
			}
		}
    }
}
