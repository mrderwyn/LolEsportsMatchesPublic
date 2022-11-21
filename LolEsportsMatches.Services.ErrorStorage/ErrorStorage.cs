using Newtonsoft.Json;

namespace LolEsportsMatches.Services.ErrorStorage
{
    /// <summary>
    /// Repository with <see cref="ErrorInfo"/> important errors to track.
    /// </summary>
    /// <remarks>
    /// Use local JSON document as storage.
    /// </remarks>
    public class ErrorStorage
    {
        private readonly string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorStorage"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <exception cref="System.ArgumentNullException">filePath</exception>
        public ErrorStorage(string filePath)
        {
            this.path = !string.IsNullOrWhiteSpace(filePath) ? filePath : throw new ArgumentNullException(nameof(filePath));
        }

        /// <summary>
        /// Gets all errors.
        /// </summary>
        /// <returns><see cref="List{ErrorInfo}"/> with all stored errors.</returns>
        public List<ErrorInfo> GetAllErrors()
        {
            using FileStream stream = File.Open(this.path, FileMode.OpenOrCreate);
            using StreamReader reader = new(stream);
            string readed = reader.ReadToEnd();
            List<ErrorInfo>? result = JsonConvert.DeserializeObject<List<ErrorInfo>>(readed);
            return result ?? new List<ErrorInfo>();
        }

        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="error">The error.</param>
        public void AddError(ErrorInfo error)
        {
            List<ErrorInfo> storedErrors = this.GetAllErrors();
            error.Id = storedErrors.MaxBy(e => e.Id)?.Id + 1 ?? 1;

            using FileStream stream = File.OpenWrite(this.path);
            using StreamWriter writer = new(stream);

            storedErrors.Add(error);
            string json = JsonConvert.SerializeObject(storedErrors, Formatting.Indented);
            writer.Write(json);
        }

        /// <summary>
        /// Removes the error.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>true if specified error has been removed, otherwise - false.</returns>
        public bool RemoveError(int id)
        {
            List<ErrorInfo> storedErrors = this.GetAllErrors();
            int result = storedErrors.RemoveAll(e => e.Id == id);
            if (result == 0)
            {
                return false;
            }

            using FileStream stream = File.Create(this.path);
            using StreamWriter writer = new(stream);
            string json = JsonConvert.SerializeObject(storedErrors, Formatting.Indented);
            writer.Write(json);
            return true;
        }
    }
}