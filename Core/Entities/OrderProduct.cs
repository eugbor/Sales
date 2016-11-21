namespace Core.Entities
{
    /// <summary>
    /// Заказ товара
    /// </summary>
    public class OrderProduct : Entity
    {
        /// <summary>
        /// Товар, который вошел в этот заказ
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Количество товара в этом заказе
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Цена за товар в этом заказе
        /// </summary>
        public decimal Price { get; set; }
    }
}
