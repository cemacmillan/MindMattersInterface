# MindMattersInterface

**MindMattersInterface** is a library designed for **RimWorld 1.5 modding** to facilitate integration with the **Mind Matters** mod. This interface provides tools for managing and interacting with the **DynamicNeeds** system introduced by Mind Matters, allowing modders to create immersive and flexible needs-based mechanics for their mods.

## Features

- **DynamicNeeds API**: Seamlessly interact with dynamic needs such as `ConstraintNeed`, `FormalityNeed`, and others.
- **Baseline Management**: Apply or modify baseline contributions to needs, enabling fine-tuned integration with apparel, tools, or environmental modifiers.
- **Satisfaction Adjustments**: Temporarily or permanently adjust satisfaction levels for needs via simple API calls.
- **Event Notifications**: Trigger custom events to influence pawn needs dynamically.
- **Experience System Integration**: Leverage Mind Matters' experience-tracking system to connect needs with broader gameplay systems.

## Key Components

- **DynamicNeeds**: The primary class for interacting with and managing dynamic needs via the API.
- **MindMattersAPI**: Provides methods for adjusting baselines, satisfying needs, and notifying events in a straightforward way.

## Usage Context

This library is exclusively for use in **RimWorld 1.5 mods** that integrate with the **Mind Matters** mod. While it includes core functionality for dynamic needs, it relies on the presence of Mind Matters to operate.

### Example Use Case

A mod such as **Slippers** might use `MindMattersAPI` to:
- Register new needs, such as a "ConstraintNeed."
- Adjust baseline contributions when specific apparel is equipped or unequipped.
- Apply temporary satisfaction to needs during events.

## Status

This library is currently in **development** and undergoing internal testing. While not yet ready for public use, its foundational structure is in place.

Future releases will include:
- Full API documentation.
- Mod examples demonstrating integration.

## Requirements

- **RimWorld** 1.5
- **Mind Matters** mod (active and loaded)

## Contributing

While the library is in early development, contributions such as feedback, testing reports, or documentation suggestions are welcome. Please raise issues or open pull requests to participate.

## License

MindMattersInterface is released under the **MIT License**. Please see [LICENSE](LICENSE) for details.

---
This project exists to simplify and expand the capabilities of RimWorld modders. For questions or suggestions, feel free to reach out via GitHub.
