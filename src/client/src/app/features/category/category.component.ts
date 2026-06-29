import { Component, ChangeDetectionStrategy, inject, computed, signal, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CategoryService } from '../../core/services/category.service';
import { ProductService } from '../../core/services/product.service';
import { ProductCardComponent } from '../../shared/components/product-card/product-card.component';
import { Category } from '../../core/models/category.model';

@Component({
  selector: 'app-category',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, ProductCardComponent],
  templateUrl: './category.component.html',
  styleUrl: './category.component.css',
})
export class CategoryComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private categoryService = inject(CategoryService);
  private productService = inject(ProductService);

  currentCategory = signal<Category | null>(null);
  breadcrumb = signal<Category[]>([]);
  subCategories = signal<Category[]>([]);

  readonly products = computed(() => {
    const cat = this.currentCategory();
    if (!cat) return [];
    return this.productService.getFiltered({ categoryIds: [cat.id] });
  });

  ngOnInit() { this.route.paramMap.subscribe(params => this.loadCategory(params)); }

  private loadCategory(params: any) {
    // Support up to 3 levels: /category/:root/:sub/:child
    const slugs = [params.get('root'), params.get('sub'), params.get('child')].filter(Boolean) as string[];
    let current: Category | undefined;
    for (const slug of slugs) {
      current = this.categoryService.getBySlug(slug);
    }
    if (!current) return;
    this.currentCategory.set(current);
    this.breadcrumb.set(this.categoryService.getCategoryPath(current.id));
    this.subCategories.set(this.categoryService.getChildren(current.id));
  }
}
