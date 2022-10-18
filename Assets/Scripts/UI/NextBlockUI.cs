using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NextBlockUI : MonoBehaviour
{
    // NOTE(Seb): There definitely must be a smarter way to do this but I'm not sure how to do it while still making it obvious which Sprite is being used
    // NOTE(Seb): and not using integers to represent the block type
    public Sprite IBlockSprite;
    public Sprite JBlockSprite;
    public Sprite LBlockSprite;
    public Sprite OBlockSprite;
    public Sprite SBlockSprite;
    public Sprite TBlockSprite;
    public Sprite ZBlockSprite;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {

    }

    public void changeNextBlockUI(BlockType blockType)
    {
        switch (blockType)
        {
            case BlockType.I_BLOCK:
                image.sprite = IBlockSprite;
                break;
            case BlockType.J_BLOCK:
                image.sprite = JBlockSprite;
                break;
            case BlockType.L_BLOCK:
                image.sprite = LBlockSprite;
                break;
            case BlockType.O_BLOCK:
                image.sprite = OBlockSprite;
                break;
            case BlockType.S_BLOCK:
                image.sprite = SBlockSprite;
                break;
            case BlockType.T_BLOCK:
                image.sprite = TBlockSprite;
                break;
            case BlockType.Z_BLOCK:
                image.sprite = ZBlockSprite;
                break;
            default:
                throw new Exception("BlockType not found");
        }
    }
}
