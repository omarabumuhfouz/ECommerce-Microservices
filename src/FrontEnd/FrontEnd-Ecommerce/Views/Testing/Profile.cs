// // Controllers/AccountController.cs
// [HttpGet]
// public async Task<IActionResult> Profile()
// {
//     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//     var user = await _userManager.FindByIdAsync(userId);
    
//     var model = new ProfileViewModel
//     {
//         Id = user.Id,
//         FirstName = user.FirstName,
//         LastName = user.LastName,
//         Email = user.Email,
//         IsActive = user.IsActive,
//         ActiveSince = user.CreatedDate.ToString("MMM yyyy"),
//         TotalOrders = await GetUserOrderCount(userId),
//         Roles = await _userManager.GetRolesAsync(user),
//         Addresses = await GetUserAddresses(userId)
//     };
    
//     return View(model);
// }

// [HttpPost]
// public async Task<IActionResult> SaveAddress(AddressViewModel model, string userId)
// {
//     if (ModelState.IsValid)
//     {
//         try
//         {
//             if (model.Id == 0)
//             {
//                 // Create new address
//                 await CreateAddress(model, userId);
//             }
//             else
//             {
//                 // Update existing address
//                 await UpdateAddress(model);
//             }
            
//             TempData["SuccessMessage"] = "Address saved successfully";
//         }
//         catch (Exception ex)
//         {
//             TempData["ErrorMessage"] = "Error saving address";
//         }
//     }
    
//     return RedirectToAction(nameof(Profile));
// }

// [HttpPost]
// public async Task<IActionResult> DeleteAddress(int addressId, string userId)
// {
//     try
//     {
//         await DeleteUserAddress(addressId, userId);
//         TempData["SuccessMessage"] = "Address deleted successfully";
//     }
//     catch (Exception ex)
//     {
//         TempData["ErrorMessage"] = "Error deleting address";
//     }
    
//     return RedirectToAction(nameof(Profile));
// }

// // Helper methods (would be in a service layer)
// private async Task<List<AddressViewModel>> GetUserAddresses(string userId)
// {
//     // Implementation to get user addresses from database
//     // This would typically use Entity Framework or your data access layer
//     return await _addressService.GetUserAddressesAsync(userId);
// }

// private async Task CreateAddress(AddressViewModel model, string userId)
// {
//     // Implementation to create new address
//     await _addressService.CreateAddressAsync(model, userId);
// }

// private async Task UpdateAddress(AddressViewModel model)
// {
//     // Implementation to update existing address
//     await _addressService.UpdateAddressAsync(model);
// }

// private async Task DeleteUserAddress(int addressId, string userId)
// {
//     // Implementation to delete address
//     await _addressService.DeleteAddressAsync(addressId, userId);
// }