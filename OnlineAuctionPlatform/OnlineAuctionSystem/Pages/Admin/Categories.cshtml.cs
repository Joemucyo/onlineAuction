using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionSystem.Data;
using OnlineAuctionSystem.Models;

namespace OnlineAuctionSystem.Pages.Admin;

[Authorize(Policy = "AdminOnly")]
public class CategoriesModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CategoriesModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Category> Categories { get; set; } = new();

    [BindProperty]
    public InputModel Input { get; set; } = new();

    [TempData]
    public string? StatusMessage { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public class InputModel
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = "bi-tag";
        public string Description { get; set; } = string.Empty;
    }

    // Load all categories from database
    public async Task OnGetAsync()
    {
        Categories = await _context.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    // Create a new auction category
    public async Task<IActionResult> OnPostCreateAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        var exists = await _context.Categories
            .AnyAsync(c => c.Name.ToLower() == Input.Name.ToLower());

        if (exists)
        {
            ErrorMessage = "A category with this name already exists.";
            await OnGetAsync();
            return Page();
        }

        var category = new Category
        {
            Name = Input.Name,
            Icon = Input.Icon,
            Description = Input.Description,
            IsActive = true
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        StatusMessage = "Category created successfully.";
        return RedirectToPage();
    }

    // Delete a category by ID
    public async Task<IActionResult> OnPostDeleteAsync(int categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        if (category is null)
        {
            ErrorMessage = "Category not found.";
            return RedirectToPage();
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        StatusMessage = "Category deleted.";
        return RedirectToPage();
    }

    // Toggle category active or inactive
    public async Task<IActionResult> OnPostToggleAsync(int categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        if (category is null)
        {
            ErrorMessage = "Category not found.";
            return RedirectToPage();
        }

        category.IsActive = !category.IsActive;
        await _context.SaveChangesAsync();
        StatusMessage = category.IsActive ? "Category activated." : "Category deactivated.";
        return RedirectToPage();
    }
}
