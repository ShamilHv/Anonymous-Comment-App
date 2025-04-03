using System.Security.Claims;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommentService(IHttpContextAccessor httpContextAccessor, ICommentRepository commentRepository, ISubjectRepository subjectRepository, IFileRepository fileRepository, IWebHostEnvironment webHostEnvironment)
        {
            _commentRepository = commentRepository;
            _subjectRepository = subjectRepository;
            _fileRepository = fileRepository;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CommentDto> CreateAdminCommentAsync(CreateAdminCommentDto createAdminCommentDto)
        {
            var parentComment = await _commentRepository.GetByIdAsync(createAdminCommentDto.ParentCommentId);
            if (parentComment == null)
            {
                throw new KeyNotFoundException("Parent comment with the id: " + createAdminCommentDto.ParentCommentId + " does not exist");
            }
            var subject = await _subjectRepository.GetSubjectByComment(parentComment);

            var comment = new Comment
            {
                SubjectId = subject.SubjectId,
                CommentText = createAdminCommentDto.CommentText,
                ParentCommentId = createAdminCommentDto.ParentCommentId,
                CreatedAt = DateTime.UtcNow,
                IsAdminComment = true,
                AdminId = GetCurrentAdminId(),

            };
            var addedComment = await _commentRepository.AddAsync(comment);
            return MapToCommentDto(addedComment);
        }

        public async Task<CommentDto> CreateAnonymousCommentAsync(CreateCommentDto createCommentDto)
        {
            Comment comment = new Comment
            {
                SubjectId = createCommentDto.SubjectId,
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
                // AdminName = commentDto.AdminName,
                Replies = comment.ChildComments.Select(MapToCommentDto).ToList()
            };
            return result;
        }
        private async Task<int> UploadFileAsync(IFormFile file)
        {
            string desktopPath = "/home/shamil/Desktop";
            var uploadsFolder = Path.Combine(desktopPath, "uploads");

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
        // private async Task<int> UploadFileAsync(IFormFile file)
        // {
        //     if (string.IsNullOrEmpty(_webHostEnvironment.WebRootPath))
        //     {
        //         throw new InvalidOperationException("WebRootPath is not configured. Check your application configuration.");
        //     }
        //     var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        //     if (!Directory.Exists(uploadsFolder))
        //     {
        //         Directory.CreateDirectory(uploadsFolder);
        //     }

        //     var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //     var filePath = Path.Combine(uploadsFolder, fileName);

        //     using (var stream = new FileStream(filePath, FileMode.Create))
        //     {
        //         await file.CopyToAsync(stream);
        //     }

        //     var fileEntity = new Models.File
        //     {
        //         FilePath = $"/uploads/{fileName}",
        //         UploadedAt = DateTime.UtcNow
        //     };

        //     var addedFile = await _fileRepository.AddAsync(fileEntity);
        //     return addedFile.FileId;
        // }
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
                IsAdminComment = comment.IsAdminComment
                // AdminName = comment.Admin?.AdminName
            };
        }
        private int GetCurrentAdminId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available");
            }

            var userIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                throw new Exception("User ID claim not found in context");
            }

            if (int.TryParse(userIdString, out int adminId))
            {
                return adminId;
            }
            else
            {
                throw new Exception("Failed to parse admin ID from claims");
            }
        }
        private string GetCurrentAdminName()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available");
            }

            var adminName = httpContext.User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(adminName))
            {
                throw new Exception("Admin name claim not found in context");
            }

            return adminName;
        }
    }
}