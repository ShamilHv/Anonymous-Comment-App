using ANONYMOUS_SURVEY.DTOs;
using ANONYMOUS_SURVEY.Models;
using ANONYMOUS_SURVEY.Repositories.Interfaces;
using ANONYMOUS_SURVEY.Services.Interfaces;

namespace ANONYMOUS_SURVEY.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CommentService(ICommentRepository commentRepository, ISubjectRepository subjectRepository, IFileRepository fileRepository, IWebHostEnvironment webHostEnvironment)
        {
            _commentRepository = commentRepository;
            _subjectRepository = subjectRepository;
            _fileRepository = fileRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<CommentDto> CreateAdminCommentAsync(CreateAdminCommentDto createAdminCommentDto, int adminId)
        {
            var subject = await _subjectRepository.GetByIdAsync(createAdminCommentDto.SubjectId);
            if (subject == null)
            {
                throw new KeyNotFoundException("Subject with the id " + createAdminCommentDto.SubjectId + "not Found");
            }
            var parentComment = await _commentRepository.GetByIdAsync(createAdminCommentDto.ParentCommentId);
            if (parentComment == null)
            {
                throw new KeyNotFoundException("Parent comment with the id: " + createAdminCommentDto.ParentCommentId + " does not exist");
            }
            var comment = new Comment
            {
                SubjectId = createAdminCommentDto.SubjectId,
                CommentText = createAdminCommentDto.CommentText,
                ParentCommentId = createAdminCommentDto.ParentCommentId,
                CreatedAt = DateTime.UtcNow,
                IsAdminComment = true,
                AdminId = adminId
            };
            var addedComment = await _commentRepository.AddAsync(comment);
            return MapToCommentDto(addedComment);
        }

        public async Task<CommentDto> CreateAnonymousCommentAsync(CreateCommentDto createCommentDto)
        {

            if (createCommentDto.ParentCommentId.HasValue)
            {
                var parentComment = await _commentRepository.GetByIdAsync(createCommentDto.ParentCommentId.Value);
                if (parentComment == null)
                {
                    throw new KeyNotFoundException("Parent comment with the id: " + createCommentDto.ParentCommentId + " does not exist");
                }
            }

            Comment comment = new Comment
            {
                SubjectId = createCommentDto.SubjectId,
                ParentCommentId = createCommentDto.ParentCommentId,
                CommentText = createCommentDto.CommentText,
                CreatedAt = DateTime.UtcNow,
                IsAdminComment = false
            };
            if (createCommentDto.File != null)
            {
                var fileId = await UploadFileAsync(createCommentDto.File);
                comment.FileId = fileId;
            }
            var addedComment = await _commentRepository.AddAsync(comment);
            return MapToCommentDto(addedComment);
        }

        public async Task<CommentDto> GetCommentByIdAsync(int commentId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
            {
                throw new KeyNotFoundException($"Comment with the id: {commentId} does not exist");
            }
            return MapToCommentDto(comment);

        }

        public async Task<IEnumerable<CommentDto>> GetCommentsBySubjectAsync(int subjectId)
        {
            var comment = await _commentRepository.GetCommentsBySubjectAsync(subjectId);
            if (comment == null)
            {
                throw new KeyNotFoundException("Not found the comments with the subject id " + subjectId);
            }
            var commentDtos = comment.Select(MapToCommentDto);
            return commentDtos;
        }

        public async Task<CommentWithRepliesDto> GetCommentWithRepliesAsync(int commentId)
        {
            var comment = await _commentRepository.GetCommentWithRepliesAsync(commentId);
            if (comment == null)
            {
                throw new KeyNotFoundException($"Comment with the id: {commentId} not found");
            }
            var commentDto = MapToCommentDto(comment);
            var result = new CommentWithRepliesDto
            {
                CommentId = commentDto.CommentId,
                SubjectId = commentDto.SubjectId,
                CommentText = commentDto.CommentText,
                CreatedAt = commentDto.CreatedAt,
                ParentCommentId = commentDto.ParentCommentId,
                HasFile = commentDto.HasFile,
                FilePath = commentDto.FilePath,
                IsAdminComment = commentDto.IsAdminComment,
                AdminName = commentDto.AdminName,
                Replies = comment.ChildComments.Select(MapToCommentDto).ToList()
            };
            return result;
        }
        private async Task<int> UploadFileAsync(IFormFile file)
        {
            if (string.IsNullOrEmpty(_webHostEnvironment.WebRootPath))
            {
                throw new InvalidOperationException("WebRootPath is not configured. Check your application configuration.");
            }
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileEntity = new Models.File
            {
                FilePath = $"/uploads/{fileName}",
                UploadedAt = DateTime.UtcNow
            };

            var addedFile = await _fileRepository.AddAsync(fileEntity);
            return addedFile.FileId;
        }
        private CommentDto MapToCommentDto(Comment comment)
        {
            return new CommentDto
            {
                CommentId = comment.CommentId,
                SubjectId = comment.SubjectId,
                CommentText = comment.CommentText,
                CreatedAt = comment.CreatedAt,
                ParentCommentId = comment.ParentCommentId,
                HasFile = comment.FileId.HasValue,
                FilePath = comment.File?.FilePath,
                IsAdminComment = comment.IsAdminComment,
                AdminName = comment.Admin?.AdminName
            };
        }
    }
}