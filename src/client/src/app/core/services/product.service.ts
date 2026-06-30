import { Injectable, computed, signal } from '@angular/core';
import { Product, ProductReview } from '../models/product.model';
import { ProductFilter, SortOption } from '../models/filter.model';
import { CategoryService } from './category.service';
import { inject } from '@angular/core';

const img = (seed: string) => `https://picsum.photos/seed/${seed}/600/600`;

export const MOCK_PRODUCTS: Product[] = [
  { id: 'p1', name: 'ProMax Ultra X Pro', slug: 'promax-ultra-x-pro', shortDescription: 'Flagship 6.8" smartphone with 200MP camera & 5G', description: 'Experience the pinnacle of mobile technology with the ProMax Ultra X Pro. Featuring a stunning 6.8-inch AMOLED display, revolutionary 200MP camera system, and blazing-fast 5G connectivity.', price: 1199, originalPrice: 1399, discount: 14, images: [img('phone1'), img('phone2')], thumbnail: img('phone1'), categoryId: 'smartphones', brand: 'TechPro', rating: 4.8, reviewCount: 1254, stock: 50, tags: ['flagship', '5G', 'camera'], attributes: { Storage: ['128GB', '256GB', '512GB'], Color: ['Midnight Black', 'Pearl Silver', 'Rose Gold'] }, isFeatured: true, isNew: true, isOnSale: true, createdAt: '2024-03-01' },
  { id: 'p2', name: 'Galaxy Z Fold Ultra', slug: 'galaxy-z-fold-ultra', shortDescription: 'Foldable smartphone with 7.6" inner display', description: 'The future of smartphones is here. Unfold a world of possibilities with the Galaxy Z Fold Ultra.', price: 1799, originalPrice: 1999, discount: 10, images: [img('fold1'), img('fold2')], thumbnail: img('fold1'), categoryId: 'smartphones', brand: 'Samsara', rating: 4.6, reviewCount: 876, stock: 30, tags: ['foldable', '5G', 'flagship'], attributes: { Storage: ['256GB', '512GB'], Color: ['Phantom Black', 'Icy Blue'] }, isFeatured: true, isNew: true, isOnSale: true, createdAt: '2024-02-15' },
  { id: 'p3', name: 'Pixel Snap 9', slug: 'pixel-snap-9', shortDescription: 'AI-powered photography & pure Android experience', description: 'Google\'s finest Pixel yet, with AI-enhanced photography and the purest Android experience available.', price: 799, images: [img('pixel1'), img('pixel2')], thumbnail: img('pixel1'), categoryId: 'smartphones', brand: 'Googleplex', rating: 4.7, reviewCount: 632, stock: 80, tags: ['AI', 'camera', 'android'], attributes: { Storage: ['128GB', '256GB'], Color: ['Obsidian', 'Porcelain', 'Sage'] }, isFeatured: false, isNew: true, isOnSale: false, createdAt: '2024-03-10' },
  { id: 'p4', name: 'BasicPhone 3G', slug: 'basicphone-3g', shortDescription: 'Reliable feature phone with long battery life', description: 'Simple, durable, and reliable. The BasicPhone 3G lasts up to 2 weeks on a single charge.', price: 49, images: [img('basic1')], thumbnail: img('basic1'), categoryId: 'feature-phones', brand: 'Nokiana', rating: 4.2, reviewCount: 210, stock: 200, tags: ['durable', 'battery'], attributes: { Color: ['Black', 'Blue', 'Red'] }, isFeatured: false, isNew: false, isOnSale: false, createdAt: '2023-01-01' },
  { id: 'p5', name: 'NexBook Pro 16', slug: 'nexbook-pro-16', shortDescription: 'M4 chip, 16" Liquid Retina, 22hr battery', description: 'The most powerful laptop for professionals. With the M4 chip and stunning Liquid Retina display, the NexBook Pro redefines what a laptop can do.', price: 2499, originalPrice: 2699, discount: 7, images: [img('laptop1'), img('laptop2')], thumbnail: img('laptop1'), categoryId: 'ultrabooks', brand: 'NexTech', rating: 4.9, reviewCount: 543, stock: 25, tags: ['M4', 'ultrabook', 'professional'], attributes: { RAM: ['16GB', '32GB', '64GB'], Storage: ['512GB', '1TB', '2TB'], Color: ['Space Gray', 'Silver', 'Midnight'] }, isFeatured: true, isNew: true, isOnSale: true, createdAt: '2024-01-20' },
  { id: 'p6', name: 'ROG Phantom X7', slug: 'rog-phantom-x7', shortDescription: 'RTX 5090, 240Hz display, ultimate gaming beast', description: 'Dominate every game with the ROG Phantom X7. Powered by the latest RTX 5090 and a blistering 240Hz QHD display.', price: 3299, images: [img('gaming1'), img('gaming2')], thumbnail: img('gaming1'), categoryId: 'gaming-laptops', brand: 'ASUS ROG', rating: 4.8, reviewCount: 321, stock: 15, tags: ['RTX 5090', '240Hz', 'gaming'], attributes: { RAM: ['32GB', '64GB'], Storage: ['1TB', '2TB'] }, isFeatured: true, isNew: true, isOnSale: false, createdAt: '2024-02-01' },
  { id: 'p7', name: 'SonicBoom Pro X', slug: 'sonicboom-pro-x', shortDescription: 'Noise-cancelling headphones, 40hr playtime', description: 'Immerse yourself in pure sound with industry-leading noise cancellation and 40 hours of battery life.', price: 349, originalPrice: 449, discount: 22, images: [img('headphone1'), img('headphone2')], thumbnail: img('headphone1'), categoryId: 'headphones', brand: 'SonicBrand', rating: 4.7, reviewCount: 2108, stock: 120, tags: ['ANC', 'wireless', 'premium'], attributes: { Color: ['Midnight Black', 'Cloud White', 'Navy Blue'] }, isFeatured: true, isNew: false, isOnSale: true, createdAt: '2023-09-01' },
  { id: 'p8', name: 'BubblePods Pro', slug: 'bubblepods-pro', shortDescription: 'True wireless earbuds with spatial audio', description: 'Premium true wireless earbuds featuring spatial audio, transparency mode, and 36-hour total battery life.', price: 249, originalPrice: 279, discount: 11, images: [img('earbuds1')], thumbnail: img('earbuds1'), categoryId: 'earbuds', brand: 'SonicBrand', rating: 4.6, reviewCount: 1876, stock: 200, tags: ['TWS', 'spatial-audio', 'ANC'], attributes: { Color: ['White', 'Black', 'Starlight'] }, isFeatured: false, isNew: false, isOnSale: true, createdAt: '2023-06-01' },
  { id: 'p9', name: 'ThunderBar 500W', slug: 'thunderbar-500w', shortDescription: '500W soundbar with Dolby Atmos & subwoofer', description: 'Fill your room with cinema-quality sound. The ThunderBar 500W delivers immersive Dolby Atmos audio.', price: 799, images: [img('speaker1')], thumbnail: img('speaker1'), categoryId: 'speakers', brand: 'AudioMax', rating: 4.5, reviewCount: 445, stock: 40, tags: ['Dolby Atmos', 'soundbar', 'home-theater'], attributes: { Color: ['Black'] }, isFeatured: false, isNew: true, isOnSale: false, createdAt: '2024-01-05' },
  { id: 'p10', name: 'SnapMaster 8K', slug: 'snapmaster-8k', shortDescription: 'Full-frame mirrorless 8K video & 60MP still', description: 'Professional-grade mirrorless camera with 60MP sensor and groundbreaking 8K video capabilities.', price: 3499, images: [img('camera1'), img('camera2')], thumbnail: img('camera1'), categoryId: 'cameras', brand: 'Lexica', rating: 4.9, reviewCount: 187, stock: 12, tags: ['mirrorless', '8K', 'professional'], attributes: { Bundle: ['Body Only', 'With 24-70mm Lens'] }, isFeatured: false, isNew: true, isOnSale: false, createdAt: '2024-02-28' },
  { id: 'p11', name: 'Velocity Slim Tee', slug: 'velocity-slim-tee', shortDescription: 'Premium cotton blend crew neck t-shirt', description: 'Crafted from premium 100% organic cotton, the Velocity Slim Tee offers unmatched comfort and style.', price: 39, originalPrice: 55, discount: 29, images: [img('shirt1'), img('shirt2')], thumbnail: img('shirt1'), categoryId: 'shirts', brand: 'UrbanThread', rating: 4.4, reviewCount: 892, stock: 500, tags: ['cotton', 'casual', 'organic'], attributes: { Size: ['XS', 'S', 'M', 'L', 'XL', 'XXL'], Color: ['White', 'Black', 'Navy', 'Grey', 'Forest Green'] }, isFeatured: false, isNew: false, isOnSale: true, createdAt: '2023-05-01' },
  { id: 'p12', name: 'Summit Puffer Jacket', slug: 'summit-puffer-jacket', shortDescription: 'Lightweight 700-fill down jacket for all seasons', description: 'Adventure-ready puffer jacket with 700-fill ethical down insulation. Packable to palm size.', price: 299, originalPrice: 380, discount: 21, images: [img('jacket1')], thumbnail: img('jacket1'), categoryId: 'jackets', brand: 'AlpineGear', rating: 4.8, reviewCount: 341, stock: 80, tags: ['down', 'packable', 'winter'], attributes: { Size: ['S', 'M', 'L', 'XL'], Color: ['Slate Blue', 'Burnt Orange', 'Olive'] }, isFeatured: true, isNew: false, isOnSale: true, createdAt: '2023-10-01' },
  { id: 'p13', name: 'Aurore Midi Dress', slug: 'aurore-midi-dress', shortDescription: 'Floral print midi dress with adjustable straps', description: 'Effortlessly elegant floral midi dress. Perfect for garden parties, brunches, and date nights.', price: 89, images: [img('dress1'), img('dress2')], thumbnail: img('dress1'), categoryId: 'dresses', brand: 'LaPetite', rating: 4.6, reviewCount: 567, stock: 120, tags: ['floral', 'summer', 'midi'], attributes: { Size: ['XS', 'S', 'M', 'L', 'XL'], Color: ['Blue Floral', 'Pink Floral', 'Yellow Floral'] }, isFeatured: true, isNew: true, isOnSale: false, createdAt: '2024-03-01' },
  { id: 'p14', name: 'Luxe Leather Tote', slug: 'luxe-leather-tote', shortDescription: 'Full-grain leather tote with laptop compartment', description: 'Timeless full-grain leather tote that seamlessly transitions from office to weekend.', price: 259, images: [img('tote1')], thumbnail: img('tote1'), categoryId: 'accessories', brand: 'LuxLeather', rating: 4.7, reviewCount: 234, stock: 60, tags: ['leather', 'tote', 'work'], attributes: { Color: ['Tan', 'Black', 'Cognac'] }, isFeatured: false, isNew: false, isOnSale: false, createdAt: '2023-08-01' },
  { id: 'p15', name: 'Velvet Cloud Sofa', slug: 'velvet-cloud-sofa', shortDescription: '3-seater velvet sofa with solid wood frame', description: 'Sink into cloud-like comfort with this luxurious 3-seater velvet sofa. Solid beech wood frame for lifetime durability.', price: 1299, originalPrice: 1599, discount: 19, images: [img('sofa1'), img('sofa2')], thumbnail: img('sofa1'), categoryId: 'living-room', brand: 'HomeVogue', rating: 4.8, reviewCount: 156, stock: 20, tags: ['velvet', 'sofa', 'luxury'], attributes: { Color: ['Emerald Green', 'Dusty Rose', 'Midnight Blue', 'Cream'] }, isFeatured: true, isNew: false, isOnSale: true, createdAt: '2023-11-01' },
  { id: 'p16', name: 'PrimaCook Set 10pc', slug: 'primacook-set-10pc', shortDescription: 'Non-stick ceramic cookware set with glass lids', description: 'Complete your kitchen with this 10-piece ceramic non-stick cookware set. PFOA-free, oven-safe to 450°F.', price: 199, originalPrice: 299, discount: 33, images: [img('cook1')], thumbnail: img('cook1'), categoryId: 'cookware', brand: 'ChefElite', rating: 4.5, reviewCount: 789, stock: 100, tags: ['non-stick', 'ceramic', 'oven-safe'], attributes: { Color: ['Graphite', 'Terracotta', 'Sage Green'] }, isFeatured: false, isNew: false, isOnSale: true, createdAt: '2023-04-01' },
  { id: 'p17', name: 'TrailBlazer X Shoes', slug: 'trailblazer-x-shoes', shortDescription: 'Waterproof trail running shoes, Gore-Tex lining', description: 'Conquer any trail with confidence. Waterproof Gore-Tex lining, Vibram outsole, and responsive cushioning.', price: 189, images: [img('shoes1'), img('shoes2')], thumbnail: img('shoes1'), categoryId: 'outdoor', brand: 'TrailKing', rating: 4.7, reviewCount: 423, stock: 90, tags: ['waterproof', 'trail', 'Gore-Tex'], attributes: { Size: ['7', '8', '9', '10', '11', '12'], Color: ['Black/Orange', 'Grey/Blue'] }, isFeatured: true, isNew: true, isOnSale: false, createdAt: '2024-01-15' },
  { id: 'p18', name: 'PowerFlex Yoga Mat', slug: 'powerflex-yoga-mat', shortDescription: '6mm thick TPE eco-friendly yoga mat', description: 'Your perfect yoga companion. 6mm premium TPE foam, non-slip surface, alignment lines, and included carrying strap.', price: 69, originalPrice: 89, discount: 22, images: [img('yoga1')], thumbnail: img('yoga1'), categoryId: 'fitness', brand: 'ZenFit', rating: 4.6, reviewCount: 1102, stock: 300, tags: ['yoga', 'eco-friendly', 'non-slip'], attributes: { Color: ['Purple', 'Teal', 'Coral', 'Black'] }, isFeatured: false, isNew: false, isOnSale: true, createdAt: '2023-07-01' },
  { id: 'p19', name: 'Atom Pro SmartWatch', slug: 'atom-pro-smartwatch', shortDescription: 'Health monitor, GPS, AMOLED, 14-day battery', description: 'Your ultimate fitness companion. Advanced health monitoring, built-in GPS, and crisp AMOLED display with 14-day battery.', price: 399, originalPrice: 499, discount: 20, images: [img('watch1'), img('watch2')], thumbnail: img('watch1'), categoryId: 'fitness', brand: 'AtomWear', rating: 4.5, reviewCount: 876, stock: 70, tags: ['smartwatch', 'GPS', 'health'], attributes: { Color: ['Black', 'Silver', 'Gold'], Size: ['40mm', '44mm'] }, isFeatured: true, isNew: false, isOnSale: true, createdAt: '2023-09-15' },
  { id: 'p20', name: 'NexBook Air 13', slug: 'nexbook-air-13', shortDescription: 'Ultra-thin 13" laptop, M3 chip, 18hr battery', description: 'Impossibly thin, impossibly capable. The NexBook Air 13 with M3 chip delivers all-day battery and pro-level performance.', price: 1299, images: [img('laptop3')], thumbnail: img('laptop3'), categoryId: 'ultrabooks', brand: 'NexTech', rating: 4.8, reviewCount: 1023, stock: 45, tags: ['M3', 'ultrabook', 'thin'], attributes: { RAM: ['8GB', '16GB', '24GB'], Storage: ['256GB', '512GB', '1TB'], Color: ['Starlight', 'Midnight', 'Silver', 'Sky Blue'] }, isFeatured: false, isNew: true, isOnSale: false, createdAt: '2024-03-05' },
  { id: 'p21', name: 'Atom Series 5 Elite', slug: 'atom-series5-elite', shortDescription: 'Professional gaming console controller', description: 'Built for elite gamers. Remappable buttons, adjustable trigger stops, hair trigger locks and a 40-hour battery.', price: 179, images: [img('controller1')], thumbnail: img('controller1'), categoryId: 'gaming', brand: 'AtomGames', rating: 4.6, reviewCount: 543, stock: 150, tags: ['gaming', 'controller', 'elite'], attributes: { Color: ['Carbon Black', 'Robot White'] }, isFeatured: false, isNew: false, isOnSale: false, createdAt: '2023-06-15' },
  { id: 'p22', name: 'Aurora Standing Desk', slug: 'aurora-standing-desk', shortDescription: 'Electric height-adjustable standing desk 55"', description: 'Work healthier with the Aurora electric sit-stand desk. Memory height presets, anti-collision, whisper-quiet motor.', price: 649, originalPrice: 849, discount: 24, images: [img('desk1')], thumbnail: img('desk1'), categoryId: 'living-room', brand: 'OfficeZen', rating: 4.7, reviewCount: 267, stock: 35, tags: ['standing-desk', 'ergonomic', 'electric'], attributes: { Color: ['White', 'Black', 'Walnut'] }, isFeatured: false, isNew: false, isOnSale: true, createdAt: '2023-08-20' },
  { id: 'p23', name: 'KitchenBoss Pro Blender', slug: 'kitchenboss-pro-blender', shortDescription: '2200W professional-grade smart blender', description: 'Crush, blend, and create with the 2200W KitchenBoss Pro. Smart programs for smoothies, soups, and nut butters.', price: 299, images: [img('blender1')], thumbnail: img('blender1'), categoryId: 'appliances', brand: 'KitchenBoss', rating: 4.8, reviewCount: 892, stock: 60, tags: ['blender', 'smart', 'professional'], attributes: { Color: ['Brushed Silver', 'Matte Black'] }, isFeatured: false, isNew: true, isOnSale: false, createdAt: '2024-02-10' },
  { id: 'p24', name: 'ClearView 4K Monitor 32"', slug: 'clearview-4k-32', shortDescription: '32" 4K OLED monitor, 144Hz, USB-C 140W', description: 'Stunning 32-inch 4K OLED display with perfect blacks, 99% DCI-P3 color accuracy, and USB-C 140W power delivery.', price: 899, originalPrice: 1099, discount: 18, images: [img('monitor1')], thumbnail: img('monitor1'), categoryId: 'gaming', brand: 'ClearView', rating: 4.9, reviewCount: 432, stock: 28, tags: ['4K', 'OLED', '144Hz'], attributes: { }, isFeatured: true, isNew: false, isOnSale: true, createdAt: '2023-12-01' },
  { id: 'p25', name: 'SleepCloud Mattress', slug: 'sleepcloud-mattress', shortDescription: 'Cooling gel memory foam mattress, 12" profile', description: 'Wake up refreshed every morning with the SleepCloud mattress. Zoned support, cooling gel foam, and breathable cover.', price: 799, originalPrice: 1200, discount: 33, images: [img('mattress1')], thumbnail: img('mattress1'), categoryId: 'bedroom', brand: 'DreamRest', rating: 4.7, reviewCount: 1543, stock: 50, tags: ['mattress', 'cooling', 'memory-foam'], attributes: { Size: ['Twin', 'Full', 'Queen', 'King', 'Cal King'] }, isFeatured: true, isNew: false, isOnSale: true, createdAt: '2023-03-01' },
];

@Injectable({ providedIn: 'root' })
export class ProductService {
  private readonly categoryService = inject(CategoryService);
  private readonly _products = signal<Product[]>(MOCK_PRODUCTS);

  readonly products = this._products.asReadonly();

  readonly brands = computed(() => {
    const set = new Set(this._products().map(p => p.brand));
    return Array.from(set).sort();
  });

  readonly priceRange = computed<[number, number]>(() => {
    const prices = this._products().map(p => p.price);
    return [Math.min(...prices), Math.max(...prices)];
  });

  getFiltered(filter: ProductFilter): Product[] {
    let results = [...this._products()];

    if (filter.search?.trim()) {
      const q = filter.search.toLowerCase();
      results = results.filter(p =>
        p.name.toLowerCase().includes(q) ||
        p.brand.toLowerCase().includes(q) ||
        p.tags.some(t => t.includes(q))
      );
    }

    if (filter.categoryIds?.length) {
      const allIds = new Set<string>();
      filter.categoryIds.forEach(id => {
        this.categoryService.getAllDescendantIds(id).forEach(did => allIds.add(did));
      });
      results = results.filter(p => allIds.has(p.categoryId));
    }

    if (filter.brands?.length) {
      results = results.filter(p => filter.brands!.includes(p.brand));
    }

    if (filter.priceRange) {
      results = results.filter(p => p.price >= filter.priceRange![0] && p.price <= filter.priceRange![1]);
    }

    if (filter.minRating) {
      results = results.filter(p => p.rating >= filter.minRating!);
    }

    if (filter.inStock) {
      results = results.filter(p => p.stock > 0);
    }

    return this.sortProducts(results, filter.sortBy);
  }

  getFeatured(): Product[] {
    return this._products().filter(p => p.isFeatured).slice(0, 8);
  }

  getNew(): Product[] {
    return this._products().filter(p => p.isNew).slice(0, 8);
  }

  getOnSale(): Product[] {
    return this._products().filter(p => p.isOnSale).slice(0, 8);
  }

  getBySlug(slug: string): Product | undefined {
    return this._products().find(p => p.slug === slug);
  }

  getById(id: string): Product | undefined {
    return this._products().find(p => p.id === id);
  }

  getRelated(product: Product, limit = 4): Product[] {
    return this._products()
      .filter(p => p.id !== product.id && p.categoryId === product.categoryId)
      .slice(0, limit);
  }

  private sortProducts(products: Product[], sortBy?: SortOption): Product[] {
    switch (sortBy) {
      case 'price_asc': return [...products].sort((a, b) => a.price - b.price);
      case 'price_desc': return [...products].sort((a, b) => b.price - a.price);
      case 'rating': return [...products].sort((a, b) => b.rating - a.rating);
      case 'newest': return [...products].sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
      case 'popular': return [...products].sort((a, b) => b.reviewCount - a.reviewCount);
      default: return [...products].sort((a, b) => (b.isFeatured ? 1 : 0) - (a.isFeatured ? 1 : 0));
    }
  }

  updateStock(productId: string, newStock: number): void {
    if (newStock < 0) newStock = 0;
    this._products.update(products => 
      products.map(p => p.id === productId ? { ...p, stock: newStock, inStock: newStock > 0 } : p)
    );
  }

  getReviews(productId: string): ProductReview[] {
    // Mocking random reviews for demonstration
    const reviews: ProductReview[] = [
      {
        id: `rev-1-${productId}`,
        productId,
        author: 'Alex Johnson',
        avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Alex',
        rating: 5,
        title: 'Absolutely fantastic product!',
        body: 'I have been using this for a few weeks now and it has completely exceeded my expectations. The build quality is superb.',
        date: new Date(Date.now() - 86400000 * 2).toISOString(), // 2 days ago
        helpful: 12
      },
      {
        id: `rev-2-${productId}`,
        productId,
        author: 'Samantha Lee',
        avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Sam',
        rating: 4,
        title: 'Great value for the price',
        body: 'Very solid performer. It does exactly what it claims to do. I deducted one star because the packaging was slightly damaged, but the product itself is flawless.',
        date: new Date(Date.now() - 86400000 * 15).toISOString(), // 15 days ago
        helpful: 5
      },
      {
        id: `rev-3-${productId}`,
        productId,
        author: 'Michael R.',
        avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Mike',
        rating: 5,
        title: 'Highly recommended',
        body: 'If you are on the fence about buying this, just do it. You will not regret it. Perfect addition to my daily routine.',
        date: new Date(Date.now() - 86400000 * 45).toISOString(), // 45 days ago
        helpful: 24
      }
    ];

    // Some products might have a slightly lower rating in mock data
    if (productId.charCodeAt(0) % 2 === 0) {
      reviews.push({
        id: `rev-4-${productId}`,
        productId,
        author: 'David W.',
        avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=David',
        rating: 3,
        title: 'It is okay, but could be better',
        body: 'Functional, but feels a bit lacking in premium features compared to competitors.',
        date: new Date(Date.now() - 86400000 * 60).toISOString(),
        helpful: 2
      });
    }

    return reviews;
  }
}
