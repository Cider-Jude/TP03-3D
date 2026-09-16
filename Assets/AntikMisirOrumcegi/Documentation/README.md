# Ancient Egyptian Spider — Low Poly Creature

Stylized low-poly giant spider with an ancient-Egyptian treatment (gold and lapis
lazuli banded abdomen, ankh inlay, usekh collar) and glowing eyes.
Rigged, animated with 10 clips, and shipped in 4 colour variants.

---

## 1. Package contents

```
AntikMisirOrumcegi/
├── Models/
│   └── Spider.fbx                       Mesh + skeleton (rest pose, no animation)
├── Animations/
│   ├── Spider@Idle.fbx                  Spider@TurnLeft.fbx
│   ├── Spider@Walk.fbx                  Spider@TurnRight.fbx
│   ├── Spider@WalkBackward.fbx          Spider@Attack.fbx
│   ├── Spider@StrafeLeft.fbx            Spider@TakeDamage.fbx
│   └── Spider@StrafeRight.fbx           Spider@Death.fbx
├── Textures/                            4 variants × 3 maps = 12 files
│   ├── Spider_Gold_Albedo.png           Spider_Gold_MetallicSmoothness.png
│   ├── Spider_Gold_Emission.png         Spider_Obsidian_*.png
│   ├── Spider_Bronze_*.png              Spider_Sand_*.png
├── Documentation/
│   ├── README.md                        (this file)
│   ├── Third-Party Notices.txt
│   └── AI-Disclosure.txt
└── Blender_source/
    └── AntikMisirOrumcegi_Blender_source.zip
```

## 2. Technical specifications

| Property | Value |
|---|---|
| Meshes | 1 (single skinned mesh) |
| Vertices | 752 |
| Polygons (quads + tris) | 764 |
| Triangles | 1,360 |
| Material slots on the mesh | 1 |
| Colour variants | 4 (Gold, Obsidian, Bronze, Sand) |
| Texture maps | 12 (3 per variant: Albedo, Metallic/Smoothness, Emission) |
| Texture resolution | 128 × 128 palette atlas, PNG, power of two |
| UV channels | 1 |
| Bones | 27 |
| Animation clips | 10 |
| Model dimensions | 1.69 m × 1.66 m × 0.91 m (leg span × length × height) |
| Scale | 1 unit = 1 metre |
| Forward axis | +Z (Unity convention) |
| Pivot | World origin, ground level, centred between the legs |
| Rig type | Generic (not Humanoid) |
| LODs | None |
| Colliders | None (add per your project's needs) |

Because the model uses a flat-shaded palette atlas, the 128 × 128 textures are
sufficient at any camera distance — every polygon samples a single solid texel.
Set the textures to **Point (no filter)** for the crispest result.

## 3. Colour variants

| Variant | Body / trim | Eyes |
|---|---|---|
| Gold | Weathered stone, gold, lapis lazuli | Red |
| Obsidian | Black obsidian, silver, turquoise | Ice blue |
| Bronze | Aged bronze, carnelian red | Amber |
| Sand | Pale limestone, faded verdigris | Toxic green |

All four share the same mesh, UVs and rig — swap the three maps on the material,
or make four materials and four prefabs.

## 4. Animation clips

Baked at 24 fps, one keyframe per frame, **root motion** on the `Root` bone.

| Clip | Frames | Length | Loops | Motion |
|---|---|---|---|---|
| Idle | 0 – 72 | 3.00 s | Yes | Breathing body, subtle leg shifts |
| Walk | 0 – 48 | 2.00 s | Yes | Forward, ~0.79 m/s |
| WalkBackward | 0 – 48 | 2.00 s | Yes | Backward, ~0.79 m/s |
| StrafeLeft | 0 – 48 | 2.00 s | Yes | Sideways left, ~0.48 m/s, heading unchanged |
| StrafeRight | 0 – 48 | 2.00 s | Yes | Sideways right, ~0.48 m/s, heading unchanged |
| TurnLeft | 0 – 48 | 2.00 s | Yes | Turn in place, ~28°/s |
| TurnRight | 0 – 48 | 2.00 s | Yes | Turn in place, ~28°/s |
| Attack | 0 – 36 | 1.50 s | No | Crouch, rear up, lunge and strike with the chelicerae |
| TakeDamage | 0 – 18 | 0.75 s | No | Recoil flinch |
| Death | 0 – 64 | 2.67 s | No | Legs curl inward, body settles and rolls |

The gait is an alternating tetrapod: legs L1, L3, R2, R4 swing while L2, L4, R1,
R3 are planted, then the groups swap. Strafe clips keep the body facing forward
and move purely with the legs.

Ground contact was verified frame by frame on every clip — no foot or body
geometry drops more than 5 mm below Y = 0 at any frame.

**If you do not want root motion**, disable *Apply Root Motion* on the Animator
and drive the transform yourself with the speeds listed above; the leg cycles are
tuned to those exact values, so foot sliding will be minimal.

## 5. Unity setup

Tested with Unity 2021.3 LTS and newer. Works in Built-in, URP and HDRP.

### 5.1 Import the model

1. Drag the `AntikMisirOrumcegi` folder into your project's `Assets` folder.
2. Select `Models/Spider.fbx` → **Rig** tab → *Animation Type*: **Generic**,
   *Avatar Definition*: **Create From This Model** → **Apply**.
3. Select each `Animations/Spider@*.fbx` → **Rig** tab → *Animation Type*:
   **Generic**, *Avatar Definition*: **Copy From Other Avatar** → pick the avatar
   from step 2 → **Apply**.

The files follow Unity's `Model@Clip.fbx` naming convention, so every clip is
imported with the correct name automatically — no manual slicing needed.

### 5.2 Animation import settings

For each `Spider@*.fbx`, on the **Animation** tab:

- **Loop Time**: on for Idle, Walk, WalkBackward, StrafeLeft, StrafeRight,
  TurnLeft, TurnRight. Off for Attack, TakeDamage, Death.
- **Root Transform Position (Y)**: Bake Into Pose, Based Upon = *Original*.
- **Root Transform Rotation**: Bake Into Pose, Based Upon = *Original*
  (leave *unbaked* on TurnLeft / TurnRight if you want the turn to drive the
  transform).
- **Root Transform Position (XZ)**: leave *unbaked* for root motion, or bake it
  in for in-place animation.

### 5.3 Material

The FBX references the Gold textures but does not carry a finished Unity
material. Create one per variant so the maps get the right colour spaces:

**Built-in / Standard shader**
| Slot | Texture | Import setting |
|---|---|---|
| Albedo | `Spider_<Variant>_Albedo` | sRGB **on** |
| Metallic | `Spider_<Variant>_MetallicSmoothness` | sRGB **off** |
| Emission | `Spider_<Variant>_Emission` | sRGB **on**, colour = white, intensity ≈ 2 |

**URP / HDRP (Lit shader)** — same assignments, using *Base Map*,
*Metallic Map* (Source: Metallic Alpha) and *Emission Map*.

On all textures set **Filter Mode: Point (no filter)** and
**Compression: None**. The atlas is tiny and point filtering keeps the flat
low-poly look crisp.

### 5.4 Prefab and demo scene

1. Drag `Spider.fbx` into a scene, assign a material, add an Animator with a
   controller containing the ten clips, save as a prefab in a `Prefabs` folder.
   Repeat for each colour variant.
2. Add a `CapsuleCollider` or `BoxCollider` sized to the body if you need
   physics — the package ships without colliders so they can be tuned per
   project.
3. The included demo scene shows all four variants on a ground plane with every
   clip playable from the Animator.

## 6. Known limitations

- The rig is **Generic**, not Humanoid — Mecanim retargeting from humanoid
  animation sets does not apply.
- No LOD group; at 1,360 triangles the model is already within a typical LOD0
  budget for a small creature.
- No colliders or gameplay scripts — this is an art asset.
- The leg segments are rigid, so in the strafe clips the two middle legs on each
  side contribute less push than the front and rear pairs. Foot contact is exact
  on the front and rear legs.
- Textures are a flat palette atlas by design; there are no normal or AO maps.
- The FBX files were exported from Blender, so the imported model carries the
  usual −90° X rotation on its transform. The mesh data itself is correct —
  +Z is forward, +Y is up — and the model behaves correctly in a scene; only
  the transform values look unusual in the Inspector. If you want a clean
  transform, drop the model under an empty GameObject and use that as the root,
  or re-export the included Blender source with "Apply Transform" enabled.

## 7. Support

Questions, bug reports and feature requests: <your support e-mail here>
