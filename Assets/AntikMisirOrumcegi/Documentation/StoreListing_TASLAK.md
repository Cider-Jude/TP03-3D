# Mağaza sayfası taslağı

> Bu bir **taslak**. Madde 3.1.a: tamamen AI üretimi açıklamalar reddedilebilir.
> Yayınlamadan önce kendi cümlelerinle yeniden yaz.

---

## Ürün adı

Ancient Egyptian Spider — Low Poly Animated Creature

## Kategori

3D → Characters → Creatures

## Kısa açıklama

Rigged low-poly giant spider with an ancient-Egyptian treatment. 10 animation
clips, 4 colour variants, 1,360 triangles, one material.

## Uzun açıklama

A stylized low-poly giant spider dressed as a temple relic: a gold-and-lapis
banded abdomen, an inlaid ankh on its back, a usekh collar at the waist, gilded
chelicerae and eight glowing eyes. Built for flat-shaded stylized projects,
mobile, and top-down or isometric games where a readable silhouette matters more
than texture detail.

**Contents**
- 1 rigged creature (1 mesh, 1 material slot, 27 bones)
- 10 animation clips as separate FBX files
- 4 colour variants (Gold, Obsidian, Bronze, Sand) with matching eye colours
- 12 texture maps (Albedo, Metallic/Smoothness, Emission per variant)
- Demo scene and prefabs
- Blender source file included

**Animation clips** (24 fps, baked, root motion included)

| Clip | Length | Loop |
|---|---|---|
| Idle | 3.00 s | yes |
| Walk | 2.00 s, ~0.79 m/s | yes |
| WalkBackward | 2.00 s, ~0.79 m/s | yes |
| StrafeLeft | 2.00 s, ~0.48 m/s | yes |
| StrafeRight | 2.00 s, ~0.48 m/s | yes |
| TurnLeft | 2.00 s, ~28°/s | yes |
| TurnRight | 2.00 s, ~28°/s | yes |
| Attack | 1.50 s | no |
| TakeDamage | 0.75 s | no |
| Death | 2.67 s | no |

Strafe clips move sideways with the legs while the body keeps its heading — the
creature does not pivot to fake it. Ground contact was verified frame by frame
on all ten clips.

**Colour variants**
- Gold — weathered stone, gold and lapis lazuli, red eyes
- Obsidian — black obsidian, silver and turquoise, ice-blue eyes
- Bronze — aged bronze and carnelian, amber eyes
- Sand — pale limestone and faded verdigris, toxic-green eyes

**Technical details**
- Vertices: 752 · Polygons: 764 · Triangles: 1,360
- Bones: 27 · Rig type: Generic (not Humanoid)
- Material slots: 1 · UV channels: 1
- Textures: 128 × 128 palette atlas, PNG, power of two, 12 files
- Maps: Albedo, Metallic/Smoothness (R = metallic, A = smoothness), Emission
- Scale: 1 unit = 1 metre. Model size 1.69 × 1.66 × 0.91 m
- Forward axis +Z, pivot at ground level between the legs
- Render pipelines: Built-in, URP and HDRP
- Unity 2021.3 LTS or newer

**Not included / limitations**
- No LOD group, no colliders, no scripts — this is an art asset.
- The rig is Generic, so humanoid Mecanim retargeting does not apply.
- Textures are a flat palette atlas by design; no normal or AO maps.

Asset created with the aid of AI; see the AI description field for details.

## Anahtar kelimeler

spider, insect, creature, low poly, lowpoly, egyptian, ancient, egypt, enemy,
monster, animated, rigged, stylized, arachnid, dungeon, temple, mobile, boss

## Görseller — hazır, `_MagazaGorselleri` klasöründe

| Dosya | Kullanım |
|---|---|
| `key_image_1950x1300.png` | Ana görsel |
| `icon_420x280.png` | Kart görseli |
| `ss_01_hero.png` | Ekran görüntüsü 1 — saldırı pozu |
| `ss_02_wireframe.png` | Ekran görüntüsü 2 — wireframe (topoloji güveni verir) |
| `ss_03_varyantlar.png` | Ekran görüntüsü 3 — 4 renk varyantı yan yana |
| `AncientEgyptianSpider_Animations.mp4` | 20 sn, 10 klip etiketli — YouTube/Vimeo'ya yükle, linki ver (madde 2.4.1.h) |

Ek olarak Unity içinden bir sahne ekran görüntüsü almanı öneririm; alıcılar
modeli gerçek bir Unity sahnesinde görmek ister.
