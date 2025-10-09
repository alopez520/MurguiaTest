using CsvHelper;
using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class TaskRepository : ITaskData
    {
        private readonly string _filePath = "tasks.csv";
        public TaskRepository()
        {
            if (!File.Exists(_filePath))
            {
                var header = "Id,Title,Description,Important,Completed,DueDate";
                File.WriteAllText(_filePath, header + Environment.NewLine);
            }
        }
        public List<TaskEntity> Get()
        {
            try
            {
                using var reader = new StreamReader(_filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                return csv.GetRecords<TaskEntity>().OrderBy(t => t.Completed).ThenByDescending(t => t.Important).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<TaskEntity> Get(string value)
        {
            try
            {
                var tasks = Get().Where(t => t.Title.Contains(value) || t.Description.Contains(value)).ToList();
                return tasks;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public TaskEntity Get(int id)
        {
            try
            {
                var task = Get().FirstOrDefault(t => t.Id == id);
                if (task == null) throw new Exception("Ocurrio un error");
                return task;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Create(TaskEntity task)
        {
            try
            {
                var tasks = Get().OrderBy(t => t.Id).ToList();
                try
                {
                    task.Id = tasks.Last().Id + 1;
                }
                catch
                {
                    task.Id = 1;
                }
                tasks.Add(task);
                SaveChanges(tasks);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Update(TaskEntity task)
        {
            try
            {
                var tasks = Get().OrderBy(t => t.Id).ToList();
                var data = tasks.FirstOrDefault(x => x.Id == task.Id);
                if (data != null)
                {
                    data.Id = task.Id;
                    data.Title = task.Title;
                    data.Description = task.Description;
                    //data.Important = task.Important;
                    //data.Completed = task.Completed;
                    data.DueDate = task.DueDate;
                }
                SaveChanges(tasks);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Delete(int id)
        {
            try
            {
                var tasks = Get().Where(t => t.Id != id).ToList();
                SaveChanges(tasks);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void updateImportant(int id, bool value)
        {
            try
            {
                var tasks = Get().OrderBy(t => t.Id).ToList();
                var data = tasks.FirstOrDefault(x => x.Id == id);
                if (data != null)
                {
                    data.Important = value;
                }
                SaveChanges(tasks);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void updateCompleted(int id, bool value)
        {
            try
            {
                var tasks = Get().OrderBy(t => t.Id).ToList();
                var data = tasks.FirstOrDefault(x => x.Id == id);
                if (data != null)
                {
                    data.Completed = value;
                }
                SaveChanges(tasks);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SaveChanges(List<TaskEntity> tasks)
        {
            try
            {
                using var writer = new StreamWriter(_filePath);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                csv.WriteHeader<TaskEntity>();
                csv.NextRecord();
                csv.WriteRecords(tasks);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
