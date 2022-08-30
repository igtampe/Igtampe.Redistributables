using Igtampe.Actions;
using Igtampe.ChopoImageHandling;
using Igtampe.ChopoSessionManager;
using Igtampe.Toffee.Common;
using Igtampe.Toffee.Common.Exceptions;
using Igtampe.Toffee.Data;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.Toffee.Actions {

    /// <summary>Agent to act upon all Categories</summary>
    public class CategoryAgent : SessionedActionAgent<ToffeeContext> {

        /// <summary>Creates a Categories Agent</summary>
        /// <param name="Context"></param>
        /// <param name="Manager"></param>
        public CategoryAgent(ToffeeContext Context, ISessionManager Manager) : base(Context, Manager) { }

        /// <summary>Gets a User's categories</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public async Task<List<Category>> GetMyCategories(Guid? SessionID)
            => await Context.GetUserCategories((await GetSession(SessionID)).Username);

        /// <summary>Creates a Category</summary>
        /// <param name="SessionID"></param>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <param name="Color"></param>
        /// <returns></returns>
        public async Task<Category> CreateCategory(Guid? SessionID, string Name, string Description, string Color) {

            var S = await GetSession(SessionID);
            var U = await Context.User.FindAsync(S.Username);

            Category C = new() {
                Name = Name, Description = Description,
                Color = Color, Creator = U,
            };

            Context.Category.Add(C);
            await Context.SaveChangesAsync();

            return C;
        }
        
        /// <summary>Updates a Category's details</summary>
        /// <param name="SessionID"></param>
        /// <param name="Category"></param>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <param name="Color"></param>
        /// <returns></returns>
        /// <exception cref="CategoryNotOwnedException">Thrown if Session is not owner of the category</exception>
        public async Task<Category> UpdateCategory(Guid? SessionID, Guid Category, string Name, string Description, string Color) {
            var S = await GetSession(SessionID);
            var C = await GetCategory(Category);
            if (C.Creator is null || C.Creator.Username != S.Username) { throw new CategoryNotOwnedException(Category); }

            C.Name = Name;
            C.Description = Description;
            C.Color = Color;
            return await SaveCategory(C);

        }

        /// <summary>Updates a Category's Icon image</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Category">Category to update the icon of</param>
        /// <param name="Icon">New Icon</param>
        /// <returns></returns>
        /// <exception cref="CategoryNotOwnedException">Thrown if Category is not owned by Session Owner</exception>
        public async Task<Category> UpdateCategoryIcon(Guid? SessionID, Guid Category, Image Icon) {
            var S = await GetSession(SessionID);
            var C = await GetCategory(Category);
            if (C.Creator is null || C.Creator.Username != S.Username) {throw new CategoryNotOwnedException(Category);}

            Image? OldIcon = C.Icon;

            C.Icon = Icon;
            await SaveCategory(C);
            if (OldIcon is not null) { Context.Image.Remove(Icon); }

            return C;
        }

        /// <summary>Gets a Category</summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// <exception cref="CategoryNotFoundException"></exception>
        public async Task<Category> GetCategory(Guid ID)
            => await Context.ApplyAutoIncludes(Context.Category).FirstOrDefaultAsync(A => A.ID == ID) 
            ?? throw new CategoryNotFoundException(ID);

        /// <summary>Gets all Categories</summary>
        /// <returns></returns>
        public async Task<List<Category>> GetAllCategories()
            => await Context.GetAllCategories();

        /// <summary>Saves a Category to the DB</summary>
        /// <param name="C"></param>
        /// <returns></returns>
        private async Task<Category> SaveCategory(Category C) {
            Context.Category.Update(C);
            await Context.SaveChangesAsync();
            return C;
        }
    }
}
