import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ProductService } from '../../core/services/product.service';
import { CategoryService } from '../../core/services/category.service';
import { ProductCardComponent } from '../../shared/components/product-card/product-card.component';
import { Product } from '../../core/models/product.model';

@Component({
  selector: 'app-home',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, ProductCardComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);

  readonly featuredProducts = this.productService.getFeatured();
  readonly newProducts = this.productService.getNew();
  readonly saleProducts = this.productService.getOnSale();
  readonly rootCategories = this.categoryService.rootCategories;

  activeTab = signal<'featured' | 'new' | 'sale'>('featured');

  get activeProducts(): Product[] {
    switch (this.activeTab()) {
      case 'new': return this.newProducts;
      case 'sale': return this.saleProducts;
      default: return this.featuredProducts;
    }
  }

  readonly heroSlides = [
    { tag: 'New Arrivals 2024', title: 'Tech That Moves You', subtitle: 'Explore the latest in smartphones, laptops & audio', cta: 'Shop Electronics', link: '/category/electronics', gradient: 'slide-1' },
    { tag: 'Summer Collection', title: 'Style Redefined', subtitle: 'Discover curated fashion for every occasion', cta: 'Shop Fashion', link: '/category/fashion', gradient: 'slide-2' },
    { tag: 'Big Sale', title: 'Up to 40% Off', subtitle: 'Limited time deals on thousands of products', cta: 'View Deals', link: '/products?sort=featured', gradient: 'slide-3' },
  ];
  currentSlide = signal(0);

  nextSlide() { this.currentSlide.update(i => (i + 1) % this.heroSlides.length); }
  prevSlide() { this.currentSlide.update(i => (i - 1 + this.heroSlides.length) % this.heroSlides.length); }
  goToSlide(i: number) { this.currentSlide.set(i); }
}
