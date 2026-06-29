import { Component, ChangeDetectionStrategy, inject, signal, computed, effect, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../core/services/product.service';
import { CategoryService } from '../../core/services/category.service';
import { ProductCardComponent } from '../../shared/components/product-card/product-card.component';
import { ProductFilter, SortOption, SORT_OPTIONS, ViewMode } from '../../core/models/filter.model';
import { CategoryTree } from '../../core/models/category.model';

@Component({
  selector: 'app-products',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, ProductCardComponent],
  templateUrl: './products.component.html',
  styleUrl: './products.component.css',
})
export class ProductsComponent implements OnInit {
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  readonly sortOptions = SORT_OPTIONS;
  readonly brands = this.productService.brands;
  readonly priceRange = this.productService.priceRange;
  readonly categoryTree = this.categoryService.categoryTree;

  // Filter state
  filter = signal<ProductFilter>({ sortBy: 'featured' });
  viewMode = signal<ViewMode>('grid');
  isSidebarOpen = signal(false);

  // Price slider values (local state, committed on change)
  minPrice = signal(0);
  maxPrice = signal(10000);

  readonly filteredProducts = computed(() => this.productService.getFiltered(this.filter()));

  // Expanded category sets
  expandedCategories = signal<Set<string>>(new Set(['electronics', 'fashion']));

  ngOnInit() {
    // Sync from query params
    const params = this.route.snapshot.queryParamMap;
    const patch: ProductFilter = { sortBy: 'featured' };
    if (params.get('search')) patch.search = params.get('search')!;
    if (params.get('sort')) patch.sortBy = params.get('sort') as SortOption;
    if (params.get('category')) patch.categoryIds = [params.get('category')!];
    this.filter.set(patch);
    const [min, max] = this.priceRange();
    this.minPrice.set(min);
    this.maxPrice.set(max);
  }

  updateFilter(patch: Partial<ProductFilter>) {
    this.filter.update(f => ({ ...f, ...patch }));
  }

  toggleCategory(id: string) {
    this.filter.update(f => {
      const ids = new Set(f.categoryIds ?? []);
      ids.has(id) ? ids.delete(id) : ids.add(id);
      return { ...f, categoryIds: ids.size ? Array.from(ids) : undefined };
    });
  }

  isCategorySelected(id: string): boolean { return !!this.filter().categoryIds?.includes(id); }

  toggleBrand(brand: string) {
    this.filter.update(f => {
      const brands = new Set(f.brands ?? []);
      brands.has(brand) ? brands.delete(brand) : brands.add(brand);
      return { ...f, brands: brands.size ? Array.from(brands) : undefined };
    });
  }

  isBrandSelected(b: string): boolean { return !!this.filter().brands?.includes(b); }

  applyPriceRange() { this.updateFilter({ priceRange: [this.minPrice(), this.maxPrice()] }); }

  toggleExpand(id: string) {
    this.expandedCategories.update(set => {
      const next = new Set(set);
      next.has(id) ? next.delete(id) : next.add(id);
      return next;
    });
  }

  isExpanded(id: string): boolean { return this.expandedCategories().has(id); }

  clearFilters() {
    const [min, max] = this.priceRange();
    this.minPrice.set(min); this.maxPrice.set(max);
    this.filter.set({ sortBy: 'featured' });
  }

  get activeFilterCount(): number {
    const f = this.filter();
    let count = 0;
    if (f.search) count++;
    if (f.categoryIds?.length) count += f.categoryIds.length;
    if (f.brands?.length) count += f.brands.length;
    if (f.priceRange) count++;
    if (f.minRating) count++;
    if (f.inStock) count++;
    return count;
  }

  setRating(r: number) { this.updateFilter({ minRating: this.filter().minRating === r ? undefined : r }); }
  toggleInStock() { this.updateFilter({ inStock: !this.filter().inStock }); }
  setSort(e: Event) { this.updateFilter({ sortBy: (e.target as HTMLSelectElement).value as SortOption }); }
  toggleSidebar() { this.isSidebarOpen.update(v => !v); }
  closeSidebar() { this.isSidebarOpen.set(false); }
}
