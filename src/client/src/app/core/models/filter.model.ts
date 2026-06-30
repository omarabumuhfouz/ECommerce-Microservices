export type SortOption = 'featured' | 'price_asc' | 'price_desc' | 'rating' | 'newest' | 'popular';
export type ViewMode = 'grid' | 'list';

export interface ProductFilter {
  search?: string;
  categoryIds?: string[];
  brands?: string[];
  priceRange?: [number, number];
  minRating?: number;
  inStock?: boolean;
  tags?: string[];
  sortBy?: SortOption;
}

export interface FilterState {
  filter: ProductFilter;
  viewMode: ViewMode;
  page: number;
  pageSize: number;
}

export const SORT_OPTIONS: { value: SortOption; label: string }[] = [
  { value: 'featured', label: 'Featured' },
  { value: 'newest', label: 'Newest Arrivals' },
  { value: 'popular', label: 'Most Popular' },
  { value: 'rating', label: 'Highest Rated' },
  { value: 'price_asc', label: 'Price: Low to High' },
  { value: 'price_desc', label: 'Price: High to Low' },
];
