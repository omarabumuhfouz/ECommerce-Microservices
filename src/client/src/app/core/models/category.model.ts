export interface Category {
  id: string;
  name: string;
  slug: string;
  description?: string;
  icon: string;
  image?: string;
  parentId: string | null;
  productCount?: number;
}

export interface CategoryTree extends Category {
  children: CategoryTree[];
  level: number;
  path: Category[]; // breadcrumb trail from root
}
