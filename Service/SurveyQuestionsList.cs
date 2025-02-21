
using TrudoseAdminPortalAPI.Data;
using TrudoseAdminPortalAPI.Dto;
using TrudoseAdminPortalAPI.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace TrudoseAdminPortalAPI.Service
{
    public class SurveyQuestionsList : ISurveyQuestionsList
    {
        private readonly ApplicationDbContext _context;

        public SurveyQuestionsList(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<List<SurveyQuestionSymptomDto>> GetSurveyDataAsync(List<int> symptomIds)
        //{
        //    if (symptomIds == null || !symptomIds.Any())
        //        return new List<SurveyQuestionSymptomDto>();

        //    var symptomIdsString = string.Join(",", symptomIds);

        //    var query = $@"
        //        SELECT 
        //            psqs.SurveyId,
        //            surv.SurveyName,
        //            psqs.QuestionId, 
        //            psq.QuestionName, 
        //            psq.QuestionType, 
        //            psq.choices,
        //            GROUP_CONCAT(DISTINCT s.SymptomsId ORDER BY s.SymptomsId SEPARATOR ', ') AS SymptomIds,
        //            GROUP_CONCAT(DISTINCT s.SymptomName ORDER BY s.SymptomName SEPARATOR ', ') AS SymptomNames
        //        FROM SymptomsNew psqs
        //        JOIN Questionnaire psq ON psqs.QuestionId = psq.QuestionId
        //        JOIN Survey surv ON psqs.SurveyId = surv.SurveyId
        //        JOIN SymptomsMaster s ON psqs.SymptomsId = s.SymptomsId
        //        WHERE psqs.SymptomsId IN ({symptomIdsString})
        //        GROUP BY psqs.SurveyId, psqs.QuestionId";

        //    var result = new List<SurveyQuestionSymptomDto>();

        //    using (var connection = _context.Database.GetDbConnection())
        //    {
        //        await connection.OpenAsync();
        //        using (var command = connection.CreateCommand())
        //        {
        //            command.CommandText = query;
        //            command.CommandType = CommandType.Text;

        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    result.Add(new SurveyQuestionSymptomDto
        //                    {
        //                        SurveyId = reader.GetInt32(0),
        //                        SurveyName = reader.GetString(1),
        //                        QuestionId = reader.GetInt32(2),
        //                        QuestionName = reader.GetString(3),
        //                        QuestionType = reader.GetString(4),
        //                        Choices = reader.IsDBNull(5) ? "" : reader.GetString(5),
        //                        SymptomIds = reader.IsDBNull(6) ? "" : reader.GetString(6),
        //                        SymptomNames = reader.IsDBNull(7) ? "" : reader.GetString(7)
        //                    });
        //                }
        //            }
        //        }
        //    }

        //    return result;
        //}


        //public async Task<List<SurveyResponseDto>> GetSurveyDataAsync(List<int> symptomIds)
        //{
        //    if (symptomIds == null || !symptomIds.Any())
        //        return new List<SurveyResponseDto>();

        //    var symptomIdsString = string.Join(",", symptomIds);

        //    var query = $@"
        //        SELECT 
        //            psqs.SurveyId,
        //            surv.SurveyName,
        //            psqs.QuestionId, 
        //            psq.QuestionName, 
        //            psq.QuestionType, 
        //            psq.choices
        //        FROM SymptomsNew psqs
        //        JOIN Questionnaire psq ON psqs.QuestionId = psq.QuestionId
        //        JOIN Survey surv ON psqs.SurveyId = surv.SurveyId
        //        WHERE psqs.SymptomsId IN ({symptomIdsString})
        //        ORDER BY psqs.SurveyId, psqs.QuestionId";

        //    var surveyDictionary = new Dictionary<int, SurveyResponseDto>();

        //    using (var connection = _context.Database.GetDbConnection())
        //    {
        //        await connection.OpenAsync();
        //        using (var command = connection.CreateCommand())
        //        {
        //            command.CommandText = query;
        //            command.CommandType = CommandType.Text;

        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    int surveyId = reader.GetInt32(0);
        //                    string surveyTitle = reader.GetString(1);
        //                    int questionId = reader.GetInt32(2);
        //                    string questionName = reader.GetString(3);
        //                    string questionType = reader.GetString(4);
        //                    string choices = reader.IsDBNull(5) ? "[]" : reader.GetString(5);

        //                    if (!surveyDictionary.ContainsKey(surveyId))
        //                    {
        //                        surveyDictionary[surveyId] = new SurveyResponseDto
        //                        {
        //                            SurveyId = surveyId,
        //                            SurveyName = surveyTitle,
        //                            Questions = new List<QuestionDto>()
        //                        };
        //                    }

        //                    surveyDictionary[surveyId].Questions.Add(new QuestionDto
        //                    {
        //                        QuestionId = questionId,
        //                        QuestionName = questionName,
        //                        QuestionType = questionType,
        //                        Choices = choices
        //                    });
        //                }
        //            }
        //        }
        //    }

        //    return surveyDictionary.Values.ToList();
        //}


        public async Task<List<SurveyResponseDto>> GetSurveyDataAsync(List<int> symptomIds)
        {
            //    if (symptomIds == null || !symptomIds.Any())
            //        return new List<SurveyResponseDto>();

            //    var symptomIdsString = string.Join(",", symptomIds);

            //    var query = $@"
            //        SELECT 
            //            psqs.SurveyId,
            //            surv.SurveyName,
            //            psqs.QuestionId, 
            //            psq.QuestionName, 
            //            psq.QuestionType, 
            //            psq.choices
            //        FROM SymptomsNew psqs
            //        JOIN Questionnaire psq ON psqs.QuestionId = psq.QuestionId
            //        JOIN Survey surv ON psqs.SurveyId = surv.SurveyId
            //        WHERE psqs.SymptomsId IN ({symptomIdsString})
            //        ORDER BY psqs.SurveyId, psqs.QuestionId";

            //    var surveyDictionary = new Dictionary<int, SurveyResponseDto>();

            //    using (var connection = _context.Database.GetDbConnection())
            //    {
            //        await connection.OpenAsync();
            //        using (var command = connection.CreateCommand())
            //        {
            //            command.CommandText = query;
            //            command.CommandType = CommandType.Text;

            //            using (var reader = await command.ExecuteReaderAsync())
            //            {
            //                while (await reader.ReadAsync())
            //                {
            //                    int surveyId = reader.GetInt32(0);
            //                    string surveyName = reader.GetString(1);
            //                    int questionId = reader.GetInt32(2);
            //                    string questionName = reader.GetString(3);
            //                    string questionType = reader.GetString(4);
            //                    string choices = reader.IsDBNull(5) ? "[]" : reader.GetString(5);

            //                    if (!surveyDictionary.ContainsKey(surveyId))
            //                    {
            //                        surveyDictionary[surveyId] = new SurveyResponseDto
            //                        {
            //                            SurveyId = surveyId,
            //                            SurveyName = surveyName,
            //                            Questions = new List<QuestionDto>()
            //                        };
            //                    }

            //                    var survey = surveyDictionary[surveyId];

            //                    // Check if the question already exists before adding
            //                    if (!survey.Questions.Any(q => q.QuestionId == questionId))
            //                    {
            //                        survey.Questions.Add(new QuestionDto
            //                        {
            //                            QuestionId = questionId,
            //                            QuestionName = questionName,
            //                            QuestionType = questionType,
            //                            Choices = choices
            //                        });
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    return surveyDictionary.Values.ToList();
            //}

            if (symptomIds == null || !symptomIds.Any())
                return new List<SurveyResponseDto>();

            var symptomIdsString = string.Join(",", symptomIds);

            var query = $@"
                SELECT 
                    sqs.survey_id,
                    sm.survey_name,
                    sqs.question_id, 
                    qm.question_name, 
                    qm.question_type, 
                    qm.question_choices,
                    sqs.order_no
                FROM survey_question_symptom_map sqs
                JOIN questions_master qm ON sqs.question_id = qm.id
                JOIN surveys_master sm ON sqs.survey_id = sm.id
                WHERE sqs.symptom_id IN ({symptomIdsString})
                ORDER BY sqs.survey_id, sqs.order_no";

            var surveyDictionary = new Dictionary<int, SurveyResponseDto>();

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int surveyId = reader.GetInt32(0);
                            string surveyName = reader.GetString(1);
                            int questionId = reader.GetInt32(2);
                            string questionName = reader.GetString(3);
                            string questionType = reader.GetString(4);
                            string choices = reader.IsDBNull(5) ? "[]" : reader.GetString(5);
                            int orderNo = reader.GetInt32(6);

                            // Check if Survey already exists, if not, create it
                            if (!surveyDictionary.ContainsKey(surveyId))
                            {
                                surveyDictionary[surveyId] = new SurveyResponseDto
                                {
                                    id = surveyId,
                                    survey_name = surveyName,
                                    questions = new List<QuestionDto>()
                                };
                            }

                            // Check if the question is already added (to remove duplicates)
                            if (!surveyDictionary[surveyId].questions.Any(q => q.question_id == questionId))
                            {
                                surveyDictionary[surveyId].questions.Add(new QuestionDto
                                {
                                    question_id = questionId,
                                    question_name = questionName,
                                    question_type = questionType,
                                    question_choices = choices,
                                    order_no = orderNo
                                });
                            }
                        }
                    }
                }
            }

            return surveyDictionary.Values.ToList();
        }
    }
}
