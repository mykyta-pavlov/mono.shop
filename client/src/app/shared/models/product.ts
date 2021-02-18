export interface IProduct {
    id: number;
    name: string;
    description: string;
    price: number;
    thumbnailUrl: string;
    imageUrls: string[];
    sizesAvailable: [string: boolean];
    productType: string;
  }
