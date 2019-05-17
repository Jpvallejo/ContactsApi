using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApi.Repository
{
    public class ContactsRepository
    {
        public ContactsApiContext ContactContext { get; set; }

        public ContactsRepository() => ContactContext = new ContactsApiContext(new DbContextOptions<ContactsApiContext>());


        public IQueryable<Contact> FindAll()
        {
            return this.ContactContext.Set<Contact>().AsNoTracking();
        }

        public Contact Get(Guid id)
        {
            return this.ContactContext.Set<Contact>().SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<Contact> FindByCondition(Expression<Func<Contact, bool>> expression)
        {
            return this.ContactContext.Set<Contact>().Where(expression).AsNoTracking();

        }

        public void Create(Contact entity)
        {
            this.ContactContext.Set<Contact>().Add(entity);
            ContactContext.SaveChangesAsync();
        }

        public void Update(Contact entity)
        {
            this.ContactContext.Set<Contact>().Update(entity);
            ContactContext.SaveChangesAsync();
        }

        public void Delete(Contact entity)
        {
            entity.Active = false;
            this.ContactContext.Set<Contact>().Update(entity); //logical delete
            ContactContext.SaveChangesAsync();
        }
    }
}

