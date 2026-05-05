using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FestivalWasteTracker.Models
{
    [Table("waste_records")]
    public class WasteRecord
    {
        [Key]
        [Column("record_id")]
        public int RecordId { get; set; }

        [Column("event_name")]
        public string? EventName { get; set; }

        [Column("waste_type")]
        public string? WasteType { get; set; }

        [Column("quantity_kg")]
        public decimal? QuantityKg { get; set; }

        [Column("reported_by")]
        public string? ReportedBy { get; set; }

        [Column("collected_date")]
        public DateTime? CollectedDate { get; set; }

        [Column("image_path")]
        public string? ImagePath { get; set; }

        [Column("status")]
        public string? Status { get; set; }

        // ⭐ VERY IMPORTANT — MATCH DB COLUMN NAME
        [Column("location_text")]
        public string? LocationName { get; set; }

        [Column("latitude")]
        public double? Latitude { get; set; }

        [Column("longitude")]
        public double? Longitude { get; set; }
    }
}