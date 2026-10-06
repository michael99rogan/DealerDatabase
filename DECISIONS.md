- Consolidation uses source authority rather than simply taking the first non-null value. Companies House is preferred for company identity and company metadata, FCA for FCA data, ICO for ICO data, SAF for SAF data, and VAT lookup results for VAT data.

- Registered and trading addresses are stored separately because they can legitimately refer to different locations. Companies House addresses are treated as registered addresses, while MarketCheck, crawler and SAF addresses are treated as trading addresses.

- Trading-address deduplication is performed only after records have already been matched into the same dealer group. It is intentionally conservative and does not form part of the entity-matching algorithm.

- Phone numbers, email addresses and website domains are normalized and deduplicated during consolidation. Website values are stored as canonical domains rather than retaining differences such as protocol or `www.` prefixes.

- VAT `NOT_FOUND` responses are treated as valid lookup outcomes rather than parsing failures. They do not produce otherwise empty normalized dealer records.

- Provenance is retained at two levels. `LineageTag` records the source records that contributed to the consolidated dealer, while `DealerFieldSource` records which source record supplied individual canonical scalar values.

- The SQLite database represents a derived consolidated snapshot rather than independently maintained application data. Each import therefore replaces the previous dealer dataset inside a transaction. This makes the import rerunnable without creating duplicate dealers and avoids relying on a single natural key that does not exist across every source.

- Child entities are cascade-deleted with their dealer during replacement so that repeated imports do not leave obsolete trading names, addresses, contacts, or provenance records behind.

- Persistence uses EF Core Code First migrations with SQLite as required by the task.

- Some source-specific fields that are not required for the consolidated dealer model are retained only in the parsed/raw source records rather than being added to the database schema. Further enrichment was deferred in favour of concentrating on matching correctness and provenance.

- More sophisticated fuzzy name/address matching and automatic resolution of remaining likely false negatives were deliberately deferred. These techniques could improve recall but would require additional validation and tuning to avoid increasing false-positive merges.