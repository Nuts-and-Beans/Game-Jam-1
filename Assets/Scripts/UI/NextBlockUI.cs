using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NextBlockUI : MonoBehaviour
{

    public Player ID;
    // NOTE(Seb): There definitely must be a smarter way to do this but I'm not sure how to do it while still making it obvious which Sprite is being used
    public Sprite IBlockSprite;
    public Sprite LBlockSprite;
    public Sprite JBlockSprite;
    public Sprite OBlockSprite;
    public Sprite SBlockSprite;
    public Sprite TBlockSprite;
    public Sprite ZBlockSprite;

    public Image image;

    private void Awake()
    {

    }

    private void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        changeNextBlockUI(Random_Spawn.GetPlayerNextBlockType(ID));
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
