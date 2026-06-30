import { Component, inject, signal, computed } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryService } from '../../../../core/services/category.service';
import { ToastService } from '../../../../core/services/toast.service';
import { CategoryTree } from '../../../../core/models/category.model';

@Component({
  selector: 'app-category-management',
  standalone: true,
  imports: [ReactiveFormsModule, NgTemplateOutlet],
  templateUrl: './category-management.component.html',
  styleUrl: './category-management.component.css'
})
export class CategoryManagementComponent {
  categoryService = inject(CategoryService);
  toastService = inject(ToastService);
  fb = inject(FormBuilder);

  // UI State
  expandedNodes = signal<Set<string>>(new Set());
  selectedCategory = signal<CategoryTree | null>(null);

  // Form
  categoryForm = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    slug: ['', [Validators.required, Validators.pattern(/^[a-z0-9-]+$/)]],
    icon: ['📁', Validators.required],
    description: ['']
  });

  get categoryTree() {
    return this.categoryService.categoryTree();
  }

  toggleNode(categoryId: string, event: Event) {
    event.stopPropagation(); // Prevent selection when toggling
    const current = new Set(this.expandedNodes());
    if (current.has(categoryId)) {
      current.delete(categoryId);
    } else {
      current.add(categoryId);
    }
    this.expandedNodes.set(current);
  }

  isExpanded(categoryId: string): boolean {
    return this.expandedNodes().has(categoryId);
  }

  selectCategory(category: CategoryTree) {
    if (this.selectedCategory()?.id === category.id) {
      // Deselect if already selected
      this.selectedCategory.set(null);
    } else {
      this.selectedCategory.set(category);
    }
    // Reset form when selection changes
    this.categoryForm.reset({ icon: '📁' });
  }

  clearSelection() {
    this.selectedCategory.set(null);
    this.categoryForm.reset({ icon: '📁' });
  }

  onSubmitCategory() {
    if (this.categoryForm.invalid) {
      this.toastService.error('Please fix the errors in the form.');
      return;
    }

    const val = this.categoryForm.getRawValue();
    const parentId = this.selectedCategory()?.id || null;

    this.categoryService.addCategory({
      name: val.name,
      slug: val.slug,
      icon: val.icon,
      description: val.description,
      parentId
    });

    const typeMsg = parentId ? 'Subcategory' : 'Root Category';
    this.toastService.success(`${typeMsg} "${val.name}" added successfully!`);
    
    // Auto-expand the parent so the new child is visible
    if (parentId) {
      const current = new Set(this.expandedNodes());
      current.add(parentId);
      this.expandedNodes.set(current);
    }

    this.categoryForm.reset({ icon: '📁' });
  }
}
