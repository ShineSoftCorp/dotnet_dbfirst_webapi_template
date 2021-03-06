﻿namespace ApiTemplate.ContosoUniversity.DAL
{
    using ApiTemplate.ContosoUniversity.DAL.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ContosoRepository
    {
        /*
         * Models folder was generated using the following command:
         * dotnet ef dbcontext scaffold "Server=(localdb)\mssqllocaldb;Database=ContosoUniversity1;Trusted_Connection=True;MultipleActiveResultSets=true" -o Models Microsoft.EntityFrameworkCore.SqlServer -c "SchoolContext" -f
         */

        private readonly SchoolContext context;

        public ContosoRepository(SchoolContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await this.context.Student.ToListAsync();
        }

        public async Task<Student> GetStudent(int id)
        {
            var student = await this.context.Student
                .Include(s => s.Enrollment)
                    .ThenInclude(e => e.Course)
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.Id == id);

            return student;
        }

        public async Task<int?> AddStudent(Student student)
        {
            var updatedStudent = await this.context.Student.AddAsync(student);
            var res = await this.context.SaveChangesAsync();
            if (res == 0)
            {
                return null;
            }
            return updatedStudent.Entity.Id;
        }

        public async Task<bool> DeleteStudent(int id)
        {
            var student = await this.context.Student
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return false;
            }

            this.context.Student.Remove(student);
            var res = await this.context.SaveChangesAsync();

            if (res == 0)
            {
                return false;
            }

            return true;
        }
    }
}
