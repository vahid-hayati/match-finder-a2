using api.Interfaces;
using api.Models;
using Image_Processing_WwwRoot.Helpers;
using Image_Processing_WwwRoot.Interfaces;

namespace api.Services;

public class PhotoService(
    IPhotoModifySaveService _photoModifySaveService,
    ILogger<PhotoService> _logger
    ) : PhotoStandardSize, IPhotoService
{
    #region Constructor and variables
    const string WwwRootUrl = "wwwroot/";
    #endregion

    /// <summary>
    /// ADD PHOTO TO DISK
    /// Store photos on disk by creating a folder based on fileName, userId, size, crop, etc. Each user will have a folder named after their db _Id.
    /// Resize and square image with 165px(navbar & thumbnail), 256px(card).
    /// Scale image to a ~300kb max size for the enlarged gallery photo.
    /// Store photo address in db as "storage/photos/user-id/resize-pixel-square/128x128/my-photo.jpg"
    /// 
    /// DELETE PHOTO FROM DISK
    /// </summary>
    /// <param name="file"></param>
    /// <param name="userId"></param>
    /// <returns>ADD: array of filePaths. DELETE: boolean</returns>
    public async Task<string[]?> AddPhotoToDiskAsync(IFormFile file, string userId)
    {
        if (file.Length > 0) // Length => Byte 301 kB => 301_000 byte 
        {
            #region Resize and/or Store Images to Disk
            // await _photoModifySaveService.Crop(file, userId, widthIn: 450, heightIn: 800);
            // await _photoModifyService.Crop_Square(file, userId, 400);
            // await _photoModifyService.CropWithOriginalSide_Square(file, userId); 
            // await _photoModifyService.ResizeByPixel(file, userId, 500, 800);
            // await _photoModifyService.ResizeByPixel_Square(file, userId, 500);
            // await _photoModifyService.ResizeImageByScale(file, userId);

            string filePath_165_sq = await _photoModifySaveService.ResizeByPixel_Square(file, userId, 165); // navbar & thumbnail
            string filePath_256_sq = await _photoModifySaveService.ResizeByPixel_Square(file, userId, 256); // card
            string filePath_enlarged = await _photoModifySaveService.ResizeImageByScale(file, userId, (int)DimensionsEnum._4_3_800x600); // enlarged photo
            // string filePath_enlarged2 = await _photoModifyService.ResizeImageByScale(formFile, userId, (int)DimensionsEnum._4_3_1280x960); // enlarged photo

            // if conversion fails
            if (filePath_165_sq is null || filePath_256_sq is null || filePath_enlarged is null)
            {
                _logger.LogError("Photo addition failed. The returned filePath is null which is not allowed.");
                return null;
            }
            #endregion Resize and Create Images to Disk

            #region Create the photo URLs and return the result
            // generate "wwwroot/storage/wwwrootphotos/user-id/resize-pixel-square/128x128/my-photo.jpg"
            return [
                filePath_165_sq.Split(WwwRootUrl)[1], // 0 Split("wwwroot/") 
                filePath_256_sq.Split(WwwRootUrl)[1], // 1
                filePath_enlarged.Split(WwwRootUrl)[1] // 2

                // string name = "amirRshaghaghi";
                // string[] names = name.Split("R"); // ["amir", "shaghaghi"];
                // names[0] // "amir"
            ];
            #endregion
        }

        return null;
    }

    public async Task<bool> DeletePhotoFromDiskAsync(Photo photo)
    {
        List<string> photoPaths = [];

        photoPaths.Add(photo.Url_165);
        photoPaths.Add(photo.Url_256);
        photoPaths.Add(photo.Url_enlarged);

        foreach (string photoPath in photoPaths)
        {
            if (File.Exists(WwwRootUrl + photoPath))
            {
                await Task.Run(() => File.Delete(WwwRootUrl + photoPath));
            }
            else
                return false;
        }

        return true;
    }
}
