using Serilog;
using basic_api.Models;
using Microsoft.Data.SqlClient;
using basic_api.Data;
using System.Reflection.Emit;

namespace basic_api.Helpers
{
    public class RetryHelper
    {
        public Entity? retryEntity;
        public MockDatabaseContext? retryContext;
        public string? opType;
        public async Task RetryAsync(
            Func<Task> operation,
            int maxRetryAttempts = 3,
            TimeSpan initialDelay = default(TimeSpan),
            double backoffMultiplier = 2.0)        
        {
            int attempt = 0;

            while (attempt < maxRetryAttempts)
            {
                try
                {
                    await operation();
                    Log.Information("{type} Operation succeeded on attempt {Attempt}", attempt + 1, opType);
                    break; 
                }
                catch (Exception ex)
                {
                    attempt++;
                    if(attempt==1) Log.Information($"BackOff Multiplier: {backoffMultiplier}");
                    Log.Error("Operation failed on attempt {Attempt}", attempt);
                    

                    // If final retry, will terminate retry loop.
                    if (attempt >= maxRetryAttempts)
                    {
                        Log.Error(ex, "Maximum retry attempts reached. Operation failed.");
                        throw; 
                    }
  
                    // Delay between retries.
                    int delayMilliseconds = (int)(initialDelay.TotalMilliseconds * Math.Pow(backoffMultiplier, attempt - 1));
                    Log.Warning($"Retry attempt {attempt+1}. Retrying in {delayMilliseconds}ms.");
                    await Task.Delay(delayMilliseconds);

                    // Action on duplicate id entered by user
                    if(opType=="create" && IsPrimaryKeyViolation(ex)) 
                    {
                        Log.Error("Reason for failure: Violation of PRIMARY KEY constraint 'PK_Entities'. Cannot insert duplicate key in object 'dbo.Entities'. The duplicate key value is (1).");
                        retryEntity.Id = Guid.NewGuid().ToString();
                        retryContext.Entities.Add(retryEntity);
                        await retryContext.SaveChangesAsync();
                        Log.Information("Entity created successfully with Id {id}", retryEntity.Id);
                        break;
                    }
                }
            }
        }


        private static bool IsPrimaryKeyViolation(Exception exception)
        {
            return exception?.InnerException is SqlException sqlException &&
                (sqlException.Number == 2627 || sqlException.Number == 2601);
        }
    }

}