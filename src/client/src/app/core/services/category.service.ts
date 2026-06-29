import { Injectable, computed, signal } from '@angular/core';
import { Category, CategoryTree } from '../models/category.model';

export const MOCK_CATEGORIES: Category[] = [
  { id: 'electronics', name: 'Electronics', slug: 'electronics', icon: '⚡', parentId: null, productCount: 45, description: 'Latest gadgets and tech' },
  { id: 'fashion', name: 'Fashion', slug: 'fashion', icon: '👗', parentId: null, productCount: 60, description: 'Trending styles & apparel' },
  { id: 'home-garden', name: 'Home & Garden', slug: 'home-garden', icon: '🏠', parentId: null, productCount: 35, description: 'Everything for your home' },
  { id: 'sports', name: 'Sports', slug: 'sports', icon: '⚽', parentId: null, productCount: 28, description: 'Sports & outdoor gear' },
  { id: 'books', name: 'Books', slug: 'books', icon: '📚', parentId: null, productCount: 120, description: 'Books, ebooks & more' },
  { id: 'beauty', name: 'Beauty', slug: 'beauty', icon: '✨', parentId: null, productCount: 42, description: 'Skincare, makeup & wellness' },
  { id: 'phones', name: 'Phones', slug: 'phones', icon: '📱', parentId: 'electronics', productCount: 15 },
  { id: 'laptops', name: 'Laptops', slug: 'laptops', icon: '💻', parentId: 'electronics', productCount: 12 },
  { id: 'audio', name: 'Audio', slug: 'audio', icon: '🎧', parentId: 'electronics', productCount: 18 },
  { id: 'cameras', name: 'Cameras', slug: 'cameras', icon: '📷', parentId: 'electronics', productCount: 10 },
  { id: 'gaming', name: 'Gaming', slug: 'gaming', icon: '🎮', parentId: 'electronics', productCount: 22 },
  { id: 'smartphones', name: 'Smartphones', slug: 'smartphones', icon: '📱', parentId: 'phones', productCount: 10 },
  { id: 'feature-phones', name: 'Feature Phones', slug: 'feature-phones', icon: '📞', parentId: 'phones', productCount: 5 },
  { id: 'gaming-laptops', name: 'Gaming Laptops', slug: 'gaming-laptops', icon: '🎮', parentId: 'laptops', productCount: 6 },
  { id: 'ultrabooks', name: 'Ultrabooks', slug: 'ultrabooks', icon: '💼', parentId: 'laptops', productCount: 6 },
  { id: 'headphones', name: 'Headphones', slug: 'headphones', icon: '🎧', parentId: 'audio', productCount: 8 },
  { id: 'speakers', name: 'Speakers', slug: 'speakers', icon: '🔊', parentId: 'audio', productCount: 7 },
  { id: 'earbuds', name: 'Earbuds', slug: 'earbuds', icon: '🎵', parentId: 'audio', productCount: 3 },
  { id: 'mens-clothing', name: "Men's", slug: 'mens-clothing', icon: '👔', parentId: 'fashion', productCount: 25 },
  { id: 'womens-clothing', name: "Women's", slug: 'womens-clothing', icon: '👗', parentId: 'fashion', productCount: 30 },
  { id: 'accessories', name: 'Accessories', slug: 'accessories', icon: '👜', parentId: 'fashion', productCount: 15 },
  { id: 'shirts', name: 'Shirts', slug: 'shirts', icon: '👕', parentId: 'mens-clothing', productCount: 10 },
  { id: 'pants', name: 'Pants', slug: 'pants', icon: '👖', parentId: 'mens-clothing', productCount: 8 },
  { id: 'jackets', name: 'Jackets', slug: 'jackets', icon: '🧥', parentId: 'mens-clothing', productCount: 7 },
  { id: 'dresses', name: 'Dresses', slug: 'dresses', icon: '👗', parentId: 'womens-clothing', productCount: 12 },
  { id: 'tops', name: 'Tops', slug: 'tops', icon: '👚', parentId: 'womens-clothing', productCount: 10 },
  { id: 'furniture', name: 'Furniture', slug: 'furniture', icon: '🛋️', parentId: 'home-garden', productCount: 20 },
  { id: 'kitchen', name: 'Kitchen', slug: 'kitchen', icon: '🍳', parentId: 'home-garden', productCount: 15 },
  { id: 'living-room', name: 'Living Room', slug: 'living-room', icon: '🛋️', parentId: 'furniture', productCount: 10 },
  { id: 'bedroom', name: 'Bedroom', slug: 'bedroom', icon: '🛏️', parentId: 'furniture', productCount: 10 },
  { id: 'cookware', name: 'Cookware', slug: 'cookware', icon: '🥘', parentId: 'kitchen', productCount: 8 },
  { id: 'appliances', name: 'Appliances', slug: 'appliances', icon: '🔌', parentId: 'kitchen', productCount: 7 },
  { id: 'outdoor', name: 'Outdoor', slug: 'outdoor', icon: '🏕️', parentId: 'sports', productCount: 15 },
  { id: 'fitness', name: 'Fitness', slug: 'fitness', icon: '💪', parentId: 'sports', productCount: 13 },
];

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private readonly _categories = signal<Category[]>(MOCK_CATEGORIES);
  readonly categories = this._categories.asReadonly();

  readonly categoryMap = computed(() => {
    const map = new Map<string, Category>();
    this._categories().forEach(c => map.set(c.id, c));
    return map;
  });

  readonly categoryTree = computed<CategoryTree[]>(() => this.buildTree(null, []));
  readonly rootCategories = computed(() => this._categories().filter(c => c.parentId === null));

  getById(id: string): Category | undefined { return this.categoryMap().get(id); }
  getBySlug(slug: string): Category | undefined { return this._categories().find(c => c.slug === slug); }
  getChildren(parentId: string): Category[] { return this._categories().filter(c => c.parentId === parentId); }

  getAllDescendantIds(categoryId: string): string[] {
    const ids: string[] = [categoryId];
    this.getChildren(categoryId).forEach(child => ids.push(...this.getAllDescendantIds(child.id)));
    return ids;
  }

  getCategoryPath(categoryId: string): Category[] {
    const path: Category[] = [];
    let current = this.getById(categoryId);
    while (current) {
      path.unshift(current);
      current = current.parentId ? this.getById(current.parentId) : undefined;
    }
    return path;
  }

  private buildTree(parentId: string | null, parentPath: Category[]): CategoryTree[] {
    return this._categories()
      .filter(c => c.parentId === parentId)
      .map(c => {
        const path = [...parentPath, c];
        return { ...c, level: parentPath.length, path, children: this.buildTree(c.id, path) } as CategoryTree;
      });
  }

  addCategory(categoryData: Partial<Category>) {
    const newCategory: Category = {
      id: categoryData.id || `cat-${Date.now()}`,
      name: categoryData.name || 'New Category',
      slug: categoryData.slug || `cat-${Date.now()}`,
      icon: categoryData.icon || '📁',
      parentId: categoryData.parentId || null,
      productCount: 0,
      description: categoryData.description || ''
    };
    this._categories.update(cats => [...cats, newCategory]);
  }
}
